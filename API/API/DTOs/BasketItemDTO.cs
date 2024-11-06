using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public List<ProductSKUValuesDTO> ProductSKUValues { get; set; }
    }
}