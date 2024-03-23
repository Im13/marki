namespace API.DTOs.Shopee
{
    public class ShopeeOrderProductsDTO
    {
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string ProductName { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal Revenue { get; set; }
        public decimal ImportPrice { get; set; }
        public decimal OrderProfit { get; set; }
    }
}