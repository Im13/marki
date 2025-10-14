namespace API.DTOs.Product
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductSKU { get; set; }
        public decimal ImportPrice { get; set; }
        public string Slug { get; set; }
        public int ProductTypeId { get; set; }
        public string Style { get; set; }
        public string Season { get; set; }
        public string Material { get; set; }
        public bool IsTrending { get; set; }

        public List<ProductOptionDTO> ProductOptions { get; set; }
        public List<ProductSKUDTO> ProductSKUs { get; set; }
        public List<PhotoDTO> Photos { get; set; }
    }
}