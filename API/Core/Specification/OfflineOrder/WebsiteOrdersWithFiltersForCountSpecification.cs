using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class WebsiteOrdersWithFiltersForCountSpecification : BaseSpecification<Order>
    {
        public WebsiteOrdersWithFiltersForCountSpecification(OrderSpecParams orderParams) : base(x =>
            string.IsNullOrEmpty(orderParams.Search) 
                || x.Id.ToString().Contains(orderParams.Search)
        )
        {
        }
    }
}