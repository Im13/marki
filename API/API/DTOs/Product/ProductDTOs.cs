namespace API.DTOs.Product
{
    public class ProductDTOs
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductSKU { get; set; }
        public decimal ImportPrice { get; set; }
        public List<ProductOptionDTO> ProductOptions { get; set; }
        public List<ProductSKUDTO> ProductSkus { get; set; }
    }
}