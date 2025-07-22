namespace Core.Entities
{
    public class CampaignWithAdsets : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public string EffectiveStatus { get; set; }
        public string FacebookId { get; set; }
        public ICollection<AdsetWithMetrics> AdSets { get; set; }
    }
}