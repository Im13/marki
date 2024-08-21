using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class OrdersWithStatusFilterForCountSpecification : BaseSpecification<OfflineOrder>
    {
        public OrdersWithStatusFilterForCountSpecification(OrderSpecParams orderParams) : base(x =>
            x.OrderStatus.Id == orderParams.StatusId
        )
        {
        }
    }
}