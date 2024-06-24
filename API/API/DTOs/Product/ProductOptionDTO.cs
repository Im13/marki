namespace API.DTOs.Product
{
    public class ProductOptionDTO
    {
        public int? Id { get; set; }
        public string OptionName { get; set; }
        public List<ProductOptionValueDTO> ProductOptionValues { get; set; }
    }
}