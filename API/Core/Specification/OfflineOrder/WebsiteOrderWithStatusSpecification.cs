using Core.Specification;
using Core.Specification.OfflineOrderSpec;

namespace Core.Entities.OrderAggregate
{
    public class WebsiteOrderWithStatusSpecification : BaseSpecification<Order>
    {
        public WebsiteOrderWithStatusSpecification(OrderSpecParams orderParams) : base(x =>
            x.OrderStatus.Id == orderParams.StatusId 
        )
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.OrderStatus);
            AddOrderByDescending(x => x.Id);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }
    }
}