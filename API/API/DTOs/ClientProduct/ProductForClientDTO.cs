using API.DTOs.ClientProduct;

namespace API.DTOs.Product
{
    public class ProductForClientDTO : ProductToReturnDTO
    {
        public int ProductTypeId { get; set; }
        public List<PhotoDTO> Photos { get; set; }
        public string Slug { get; set; }
        public List<ProductSKUForClientDTO> ProductSkus { get; set; }
    }
}