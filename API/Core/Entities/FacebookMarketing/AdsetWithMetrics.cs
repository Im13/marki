namespace Core.Entities
{
    public class AdsetWithMetrics : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string EffectiveStatus { get; set; }
        public decimal DailyBudget { get; set; }
        public string FacebookId { get; set; }
        
        // Navigation properties
        public int CampaignId { get; set; }
        public CampaignWithAdsets Campaign { get; set; }
        
        // Vì 1-1 relationship với Metrics, nên gộp vào
        public decimal Spend { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public decimal Ctr { get; set; }
        public decimal Cpc { get; set; }
        public int Reach { get; set; }
        public decimal Frequency { get; set; }
        public DateTime MetricsDateStart { get; set; }
        public DateTime MetricsDateStop { get; set; }
    }
}