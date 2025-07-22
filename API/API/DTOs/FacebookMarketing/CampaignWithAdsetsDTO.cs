namespace API.DTOs
{
    public class CampaignWithAdsetsDTO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public string EffectiveStatus { get; set; }
        public string FacebookId { get; set; }
        public ICollection<AdsetWithMetricsDTO> AdSets { get; set; }
    }
}