namespace Core.Entities.ShopeeOrder
{
    public class ShopeeProduct : BaseEntity
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductPropertySKU { get; set; }
        public string ProductProperty { get; set; }
        public decimal Cost { get; set; }
        public decimal ShopDiscount { get; set; }
        public decimal ShopeeSale { get; set; }
        public decimal TotalShopSale { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public int ReturnedQuantity { get; set; }
        public decimal TotalSellingPrice { get; set; }
    }
}