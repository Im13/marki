namespace Core.Entities.OrderAggregate
{
    public class OnlineOrderItem : BaseEntity
    {
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public decimal DiscountAmout { get; set; }
        public decimal TotalItemPrice { get; set; }
    }
}