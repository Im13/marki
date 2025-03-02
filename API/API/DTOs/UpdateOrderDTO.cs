using API.DTOs.AdminOrder;
using Core.Entities.Identity;

namespace API.DTOs
{
    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public ShipToAddressDTO ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public string Status { get; set; }
        public OfflineOrderStatusDTO OrderStatus { get; set; }
        public string Fullname { get; set; }
        public int CityOrProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
    }
}