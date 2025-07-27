using System.Text.Json.Serialization;
using Core.Enums;

namespace Core.Entities
{
    public class CampaignWithAdsets
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime? StartTime { get; set; }

        [JsonPropertyName("effective_status")]
        public string EffectiveStatus { get; set; }

        [JsonPropertyName("objective")]
        public string Objective { get; set; }

        public CampaignObjective CampaignObjective { get; set; }    
        public ICollection<AdsetWithMetrics> AdSets { get; set; }
    }
}