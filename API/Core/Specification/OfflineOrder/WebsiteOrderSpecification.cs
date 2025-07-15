using Core.Specification;
using Core.Specification.OfflineOrderSpec;

namespace Core.Entities.OrderAggregate
{
    public class WebsiteOrderSpecification : BaseSpecification<Order>
{
    public WebsiteOrderSpecification(OrderSpecParams orderParams) 
        : base(x =>
            (!orderParams.StatusId.HasValue || x.OrderStatus.Id == orderParams.StatusId) &&
            (string.IsNullOrEmpty(orderParams.Search) || x.Id.ToString().Contains(orderParams.Search))
        )
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.OrderStatus);
        AddOrderByDescending(x => x.Id);
        ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
    }
}
}