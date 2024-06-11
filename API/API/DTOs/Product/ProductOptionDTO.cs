namespace API.DTOs.Product
{
    public class ProductOptionDTO
    {
        public string OptionName { get; set; }
        public List<ProductOptionValueDTO> ProductOptionValues { get; set; }
    }
}