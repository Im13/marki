namespace Core.Entities.Recommendation
{
    public class ProductCoOccurrence : BaseEntity
    {
        public int ProductId1 { get; set; }
        public int ProductId2 { get; set; }
        public int CoOccurrenceCount { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        public Product Product1 { get; set; }
        public Product Product2 { get; set; }
    }
}