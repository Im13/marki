namespace API.DTOs.Shopee
{
    public class ShopeeOrderDTO
    {
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string ShipmentCode { get; set; }
        public string ShippingCompany { get; set; }
        public string ReturnStatus { get; set; }
        public decimal TotalOrderValue { get; set; }
        public decimal ShopVoucher { get; set; }
        public decimal ShopeeCoinReturn { get; set; }
        public decimal ShopeeVoucher { get; set; }
        public decimal ShopeeComboDiscount { get; set; }
        public decimal ShopComboDiscount { get; set; }
        public decimal EstimatedShippingFee { get; set; }
        public decimal CustomerShippingFee { get; set; }
        public decimal EstimatedShoppingFeeShopeeDiscount { get; set; }
        public decimal ReturnOrderFee { get; set; }
        public decimal TotalOrderCustomerPaid { get; set; }
        public string OrderCompletedDate { get; set; }
        public string OrderPaidDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal FixedFee { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal PaymentFee { get; set; }
        public decimal Deposit { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string AddressDetails { get; set; }
        public string Note { get; set; }
        public IReadOnlyList<ShopeeOrderProductDTO> Products { get; set; }
    }
}