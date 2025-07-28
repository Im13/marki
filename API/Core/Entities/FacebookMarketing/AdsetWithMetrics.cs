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
        
        public decimal Spend { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public decimal Ctr { get; set; }
        public decimal Cpc { get; set; }
        public int Reach { get; set; }
        public decimal Frequency { get; set; }

        // The number of times your ad achieved an outcome, based on the objective and settings you selected.
        public string Results { get; set; }
        public DateTime MetricsDateStart { get; set; }
        public DateTime MetricsDateStop { get; set; }
    }
}