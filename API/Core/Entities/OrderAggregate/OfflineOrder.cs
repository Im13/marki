namespace Core.Entities.OrderAggregate
{
    public class OfflineOrder : BaseEntity
    {
        public ICollection<OfflineOrderSKUs> OfflineOrderSKUs { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public string OrderNote { get; set; }
        public DateTime DateCreated { get; set; }
        public int OrderCareStaffId { get; set; }
        public int CustomerCareStaffId { get; set; }
        public Customer Customer { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Address { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public Ward Ward { get; set; }
    }
}