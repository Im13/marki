using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class ProductOptionValues : BaseEntity
    {
        public string ValueName { get; set; }
        public int ValueTempId { get; set; }
        [JsonIgnore]
        public ProductOptions ProductOption { get; set; }
    }
}