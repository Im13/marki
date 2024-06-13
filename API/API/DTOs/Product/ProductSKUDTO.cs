namespace API.DTOs.Product
{
    public class ProductSKUDTO
    {
        public string Sku { get; set; }
        public decimal ImportPrice { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Barcode { get; set; }
        public int Weight { get; set; }
        public int Quantity { get; set; }
        // public List<ProductSKUValuesDTO> ProductSKUValues { get; set; }
        public List<ProductOptionValueDTO> ProductOptionValue { get; set; }
    }
}