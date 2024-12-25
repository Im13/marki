using API.DTOs.AdminOrder;

namespace API.DTOs
{
    public class OnlineOrderDTO
    {
        public decimal ShippingFee { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        public DateTime DateCreated { get; set; }
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public string CustomerEmail { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public int CustomerId { get; set; }
        public int OrderCareStaffId { get; set; }
        public OrderStatusDTO OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
    }
}