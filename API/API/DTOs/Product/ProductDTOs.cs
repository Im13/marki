namespace API.DTOs.Product
{
    public class ProductDTOs : ProductToReturnDTO
    {
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductSKU { get; set; }
        public decimal ImportPrice { get; set; }
        public List<ProductOptionSKUDTO> ProductOptionSKUs { get; set; }
        public List<ProductOptionDTO> ProductOptions { get; set; }
        public List<ProductSKUDTO> ProductSkus { get; set; }
    }
}