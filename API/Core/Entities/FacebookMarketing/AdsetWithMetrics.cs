using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class AdsetWithMetrics
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("effective_status")]
        public string Effective_Status { get; set; }

        [JsonPropertyName("daily_budget")]
        public decimal DailyBudget { get; set; }

        // Navigation properties
        public string CampaignId { get; set; }
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