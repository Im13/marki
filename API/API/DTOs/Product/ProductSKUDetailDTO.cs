namespace API.DTOs.Product
{
    public class ProductSKUDetailDTO
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }
        public float Weight { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public List<ProductSKUValuesDTO> ProductSKUValues { get; set; }
    }
}