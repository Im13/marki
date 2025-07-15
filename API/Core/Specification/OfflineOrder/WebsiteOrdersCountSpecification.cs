using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class WebsiteOrdersCountSpecification : BaseSpecification<Order>
    {
        public WebsiteOrdersCountSpecification(OrderSpecParams orderParams)
        : base(x =>
            (orderParams.StatusId == null || x.OrderStatus.Id == orderParams.StatusId) &&
            (string.IsNullOrEmpty(orderParams.Search) || x.Id.ToString().Contains(orderParams.Search))
        )
    {
    }
    }
}