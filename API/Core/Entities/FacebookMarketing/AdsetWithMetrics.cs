namespace Core.Entities
{
    public class AdsetWithMetrics
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        // Metrics
        public string Spend { get; set; }
        public string Impressions { get; set; }
        public string Clicks { get; set; }
        public string CTR { get; set; }
        public string CPC { get; set; }
        public string LinkClicks { get; set; }
        public string Reach { get; set; }
        public string Frequency { get; set; }
        public string WebsitePurchaseROAS { get; set; }
    }
}