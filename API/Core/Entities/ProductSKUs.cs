using Core.Entities;

namespace Core
{
    public class ProductSKUs : BaseEntity
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }
        public float Weight { get; set; }
        public List<ProductSKUValues> ProductSKUValues { get; set; }
    }
}