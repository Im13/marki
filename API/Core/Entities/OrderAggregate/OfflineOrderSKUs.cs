namespace Core.Entities.OrderAggregate
{
    public class OfflineOrderSKUs : BaseEntity
    {
        public ProductSKUs ProductSKU { get; set; }
        public int? ProductSkuId { get; set; }
        public int Quantity { get; set; }
    }
}