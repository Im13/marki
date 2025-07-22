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
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _adAccountId;

        public FacebookMarketingService(IUnitOfWork unitOfWork, HttpClient httpClient, string accessToken, string adAccountId, IOptions<FacebookSettings> config)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _accessToken = config.Value.AccessToken ?? accessToken;
            _adAccountId = config.Value.AdAccountId ?? adAccountId;
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

        public async Task<List<AdsetWithMetrics>> GetAdsetsByCampaignIdAsync(int campaignId)
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

        public async Task<Dictionary<string, string>> GetAdsetInsightsAsync(int adsetId, DateTime since, DateTime until)
        {
            string timeRange = $"{{\"since\":\"{since:yyyy-MM-dd}\",\"until\":\"{until:yyyy-MM-dd}\"}}";

            var url = $"https://graph.facebook.com/v23.0/{adsetId}/insights" +
                      $"?fields=spend,impressions,clicks,ctr,cpc,link_clicks,reach,frequency,website_purchase_roas" +
                      $"&time_range={Uri.EscapeDataString(timeRange)}" +
                      $"&access_token={_accessToken}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await JsonSerializer.DeserializeAsync<FacebookResponse<Dictionary<string, string>>>(
                await response.Content.ReadAsStreamAsync());

            return result?.Data?.FirstOrDefault() ?? new Dictionary<string, string>();
        }

        private class FacebookResponse<T>
        {
            [JsonPropertyName("data")]
            public List<T> Data { get; set; }
        }
    }
}