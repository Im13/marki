using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;

namespace API.Controllers.Admin
{
    public class FacebookMarketingService : IFacebookMarketingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _adAccountId;

        public FacebookMarketingService(IHttpClientFactory httpClientFactory, IOptions<FacebookSettings> config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _accessToken = config.Value.AccessToken;
            _adAccountId = config.Value.AdAccountId;
        }

        public async Task<List<CampaignWithAdsets>> GetActiveCampaignsWithAdsetsInsightsAsync(DateTime since, DateTime until)
        {
            var campaigns = await GetActiveCampaignsAsync();

            foreach (var campaign in campaigns)
            {
                var adsets = await GetAdsetsByCampaignIdAsync(campaign.Id);

                foreach (var adset in adsets)
                {
                    var metrics = await GetAdsetInsightsAsync(adset.Id, since, until);

                    if (decimal.TryParse(metrics.GetValueOrDefault("spend"), out decimal spend))
                        adset.Spend = spend;

                    if (int.TryParse(metrics.GetValueOrDefault("impressions"), out int impressions))
                        adset.Impressions = impressions;

                    if (int.TryParse(metrics.GetValueOrDefault("clicks"), out int clicks))
                        adset.Clicks = clicks;

                    if (decimal.TryParse(metrics.GetValueOrDefault("ctr"), out decimal ctr))
                        adset.Ctr = ctr;

                    if (decimal.TryParse(metrics.GetValueOrDefault("cpc"), out decimal cpc))
                        adset.Cpc = cpc;

                    if (int.TryParse(metrics.GetValueOrDefault("reach"), out int reach))
                        adset.Reach = reach;

                    if (decimal.TryParse(metrics.GetValueOrDefault("frequency"), out decimal frequency))
                        adset.Frequency = frequency;

                    adset.MetricsDateStart = since;
                    adset.MetricsDateStop = until;
                    adset.CampaignId = campaign.Id;
                }

                campaign.AdSets = adsets;
            }

            return campaigns;
        }

        public async Task<List<CampaignWithAdsets>> GetActiveCampaignsAsync()
        {
            var url = $"https://graph.facebook.com/v23.0/act_{_adAccountId}/campaigns" +
                      $"?fields=id,name,status,effective_status" +
                      $"&effective_status=['ACTIVE']" +
                      $"&access_token={_accessToken}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await JsonSerializer.DeserializeAsync<FacebookResponse<CampaignWithAdsets>>(
                await response.Content.ReadAsStreamAsync());

            return result?.Data ?? new List<CampaignWithAdsets>();
        }

        public async Task<List<AdsetWithMetrics>> GetAdsetsByCampaignIdAsync(string campaignId)
        {
            var url = $"https://graph.facebook.com/v23.0/{campaignId}/adsets" +
                      $"?fields=id,name,status,effective_status" +
                      $"&access_token={_accessToken}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await JsonSerializer.DeserializeAsync<FacebookResponse<AdsetWithMetrics>>(
                await response.Content.ReadAsStreamAsync());

            return result?.Data ?? new List<AdsetWithMetrics>();
        }

        public async Task<Dictionary<string, string>> GetAdsetInsightsAsync(string adsetId, DateTime since, DateTime until)
        {
            var timeRange = $"{{'since':'{since:yyyy-MM-dd}','until':'{until:yyyy-MM-dd}'}}";

            var url = $"https://graph.facebook.com/v23.0/{adsetId}/insights" +
                      $"?fields=spend,impressions,clicks,ctr,cpc,reach,frequency,website_purchase_roas" +
                      $"&time_range={Uri.EscapeDataString(timeRange)}" +
                      $"&access_token={_accessToken}";

            try
            {
                // In ra URL để debug (remove trong production)
                Console.WriteLine($"Request URL: {url}");

                var response = await _httpClient.GetAsync(url);

                // Đọc error message nếu có
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Facebook API error: Status: {response.StatusCode}, Content: {errorContent}");
                }

                var result = await JsonSerializer.DeserializeAsync<FacebookResponse<Dictionary<string, string>>>(
                    await response.Content.ReadAsStreamAsync());

                return result?.Data?.FirstOrDefault() ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting insights for adset {adsetId}: {ex.Message}");
            }
        }

        private class FacebookResponse<T>
        {
            [JsonPropertyName("data")]
            public List<T> Data { get; set; }
        }
    }
}