namespace Core.Entities
{
    public class CampaignWithAdsets
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<AdsetWithMetrics> Adsets { get; set; } = new();
    }
}