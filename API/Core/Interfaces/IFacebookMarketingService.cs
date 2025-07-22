using Core.Entities;

namespace Core.Interfaces
{
    public interface IFacebookMarketingService
    {
        Task<List<CampaignWithAdsets>> GetActiveCampaignsWithAdsetsInsightsAsync(DateTime since, DateTime until);
        Task<List<CampaignWithAdsets>> GetActiveCampaignsAsync();
        Task<List<AdsetWithMetrics>> GetAdsetsByCampaignIdAsync(int campaignId);
        Task<Dictionary<string, string>> GetAdsetInsightsAsync(int adsetId, DateTime since, DateTime until);
    }
}