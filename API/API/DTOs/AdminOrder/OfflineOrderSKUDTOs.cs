using API.DTOs.Product;

namespace API.DTOs.AdminOrder
{
    public class OfflineOrderSKUDTOs
    {
        public ProductSKUDTO ProductSKU { get; set; }
        public int Quantity { get; set; }
    }
}