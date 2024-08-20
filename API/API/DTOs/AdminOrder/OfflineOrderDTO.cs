using API.DTOs.Product;

namespace API.DTOs.AdminOrder
{
    public class OfflineOrderDTO
    {
        public int Id { get; set; }
        public List<OfflineOrderSKUDTOs> OfflineOrderSKUs { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        public DateTime DateCreated { get; set; }
        public int OrderCareStaffId { get; set; }
        public int CustomerCareStaffId { get; set; }
        public CustomerDTO Customer { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public OfflineOrderStatusDTO OrderStatus { get; set; }
    }
}