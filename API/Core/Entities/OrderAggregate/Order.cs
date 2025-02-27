namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(List<OrderItem> orderItems, 
            string buyerEmail, 
            Address shipToAddress, 
            DeliveryMethod deliveryMethod, 
            decimal subTotal, 
            OrderSources source,
            decimal shippingFee,
            decimal orderDiscount,
            decimal bankTransferedAmount,
            decimal extraFee,
            decimal total,
            string orderNote,
            OfflineOrderStatus orderStatus)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subTotal;
            Source = source;
            ShippingFee = shippingFee;
            OrderDiscount = orderDiscount;
            BankTransferedAmount = bankTransferedAmount;
            ExtraFee = extraFee;
            Total = total;
            OrderNote = orderNote;
            OrderStatus = orderStatus;
        }

        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        // public OrderStatus Status { get; set; } = OrderStatus.New;
        public OfflineOrderStatus OrderStatus { get; set; }
        public OrderSources Source { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal BankTransferedAmount { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal Total { get; set; }
        public string OrderNote { get; set; }
        
        //Using with Stripe
        public string PaymentIntentId { get; set; }

        //When using automapper, it will map this method to property called Total
        public decimal GetTotal() 
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}