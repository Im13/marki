namespace API.DTOs.Product
{
    public class ProductOptionDTO
    {
        public string Name { get; set; }
        public ICollection<ProductOptionValueDTO> ProductOptionValues { get; set; }
    }
}