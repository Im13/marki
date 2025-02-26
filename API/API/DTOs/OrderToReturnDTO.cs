
using Core.Entities.OrderAggregate;

namespace API.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        public IReadOnlyList<OrderItemDTO> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        // public decimal Total { get; set; }
        public string Status { get; set; }
    }
}