using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<OfflineOrder> CreateOfflineOrder(OfflineOrder order, int provinceId, int wardId, int districtId);
        Task<OfflineOrder> UpdateOfflineOrder(OfflineOrder order, List<OfflineOrderSKUs> currentListSkus);
        Task<OfflineOrder> GetOrderAsync(int id);
        Task<OfflineOrder> UpdateStatus(OfflineOrder order, int statusId);
        Task<OfflineOrder> GetOrderWithStatusAsync(int orderId);
        Task<List<OfflineOrder>> GetOrdersByStatusIdAsync(int statusId);
    }
}