using Core.Entities;

namespace Core
{
    public class ProductSKUs : BaseEntity
    {
        public int ProductId { get; set; }
        public int SKUId { get; set; }
        public int SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }
        public Product Product { get; set; }
    }
}