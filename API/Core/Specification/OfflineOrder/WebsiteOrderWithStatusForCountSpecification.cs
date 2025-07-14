using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class WebsiteOrderWithStatusForCountSpecification : BaseSpecification<Order>
    {
        public WebsiteOrderWithStatusForCountSpecification(OrderSpecParams orderParams) : base(x =>
            x.OrderStatus.Id == orderParams.StatusId
        )
        {
        }
    }
}