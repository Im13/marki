using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class OrderSpecification : BaseSpecification<OfflineOrder>
    {
        public OrderSpecification(OrderSpecParams orderParams) : base(x => 
            string.IsNullOrEmpty(orderParams.Search) 
                || x.Id.ToString().Contains(orderParams.Search) 
                || x.Customer.PhoneNumber.ToString().Contains(orderParams.Search)
        )
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.OfflineOrderSKUs);
            AddOrderByDescending(x => x.Id);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }
    }
}