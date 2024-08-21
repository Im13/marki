using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class OrderWithStatusFilterSpecification : BaseSpecification<OfflineOrder>
    {
        public OrderWithStatusFilterSpecification(OrderSpecParams orderParams) : base(x =>
            x.OrderStatus.Id == orderParams.StatusId
        )
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.OfflineOrderSKUs);
            AddOrderByDescending(x => x.Id);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }
    }
}