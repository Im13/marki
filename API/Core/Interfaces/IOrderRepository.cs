using Core.Entities.OrderAggregate;
using Core.Specification;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OfflineOrder>> GetOrdersWithSpec(ISpecification<OfflineOrder> spec);
        Task<List<Order>> GetWebsiteOrdersWithSpec(ISpecification<Order> spec);
        Task<Order> GetWebsiteOrderById(int id);
    }
}