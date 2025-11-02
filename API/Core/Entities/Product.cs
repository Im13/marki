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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ICollection<ProductOptions> ProductOptions { get; set; }
        public ICollection<ProductSKUs> ProductSKUs { get; set; }
        public ICollection<Photo> Photos { get; set; }

        // Properties for Content-Based Filtering
        public string Style { get; set; }        
        public string Season { get; set; }       
        public string Material { get; set; }     
        public bool IsTrending { get; set; }     
    }
}