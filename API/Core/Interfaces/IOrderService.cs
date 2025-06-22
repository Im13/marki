using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress, decimal shippingFee, decimal orderDiscount, decimal bankTransferedAmount, decimal extraFee, decimal total, string orderNote);
        Task<Order> CreateOrderFromAdminAsync(Order order);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<OfflineOrder> CreateOfflineOrder(OfflineOrder order, int provinceId, int wardId, int districtId);
        Task<OfflineOrder> UpdateOfflineOrder(OfflineOrder order, List<OfflineOrderSKUs> currentListSkus);
        Task<Order> UpdateOrder(Order order, List<OrderItem> items);
        Task<OfflineOrder> GetOrderAsync(int id);
        Task<OfflineOrder> UpdateStatus(OfflineOrder order, int statusId);
        Task<Order> UpdateWebsiteOrderStatus(Order order, int statusId);
        Task<OfflineOrder> GetOrderWithStatusAsync(int orderId);
        Task<Order> GetWebsiteOrderWithStatusAsync(int orderId);
        Task<List<OfflineOrder>> GetOrdersByStatusIdAsync(int statusId);
        Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id);
    }
}