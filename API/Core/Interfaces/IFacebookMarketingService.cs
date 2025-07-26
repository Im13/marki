using Core.Entities;

namespace Core.Interfaces
{
    public interface IFacebookMarketingService
    {
        Task<List<CampaignWithAdsets>> GetActiveCampaignsWithAdsetsInsightsAsync(DateTime since, DateTime until);
        Task<List<CampaignWithAdsets>> GetActiveCampaignsAsync();
        Task<List<AdsetWithMetrics>> GetAdsetsByCampaignIdAsync(string campaignId);
        Task<Dictionary<string, string>> GetAdsetInsightsAsync(string adsetId, DateTime since, DateTime until);
    }
}