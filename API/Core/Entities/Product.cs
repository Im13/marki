namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductSKU { get; set; }
        public decimal ImportPrice { get; set; }
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ICollection<ProductOptions> ProductOptions { get; set; }
        public ICollection<ProductSKUs> ProductSKUs { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}