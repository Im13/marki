namespace API.DTOs
{
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        public decimal Subtotal { get; set; }

        public string Fullname { get; set; }
        public int CityOrProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
    }
}