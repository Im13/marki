namespace API.DTOs.Product
{
    public class ProductDTOs
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
        public ICollection<ProductOptionDTO> ProductOptions { get; set; }
        public ICollection<ProductSKUDTO> ProductSkus { get; set; }
    }
}