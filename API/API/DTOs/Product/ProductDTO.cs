namespace API.DTOs
{
    public class ProductDTO : ProductToReturnDTO
    {
        public string ProductSKU { get; set; }
        public decimal ImportPrice { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
    }
}