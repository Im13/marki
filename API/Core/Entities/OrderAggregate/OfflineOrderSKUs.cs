namespace Core.Entities.OrderAggregate
{
    public class OfflineOrderSKUs : BaseEntity
    {
        public ProductSKUs ProductSKU { get; set; }
        
        public int Quantity { get; set; }
    }
}