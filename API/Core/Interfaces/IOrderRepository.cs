using Core.Entities.OrderAggregate;
using Core.Specification;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OfflineOrder>> GetOrdersWithSpec(ISpecification<OfflineOrder> spec);
    }
}