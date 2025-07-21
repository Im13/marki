namespace API.DTOs
{
    public class CampaignWithAdsetsDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<AdsetWithMetricsDTO> Adsets { get; set; }
    }
}