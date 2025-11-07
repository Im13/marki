using API.DTOs.ClientProduct;

namespace API.DTOs.Product
{
    public class ProductForClientDTO : ProductToReturnDTO
    {
        public int ProductTypeId { get; set; }
        public string PictureUrl { get; set; }
        public List<PhotoDTO> Photos { get; set; }
        public string Slug { get; set; }
        public string ProductSKU { get; set; }

        public List<ProductSKUForClientDTO> ProductSkus { get; set; }
        public List<ProductOptionDTO> ProductOptions { get; set; }
        public string PriceDisplay { get; set; }
    }
}