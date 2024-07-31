using Core.Entities.OrderAggregate;

namespace Core.Specification.OfflineOrderSpec
{
    public class OrdersWithFiltersForCountSpecification : BaseSpecification<OfflineOrder>
    {
        public OrdersWithFiltersForCountSpecification(OrderSpecParams orderParams) : base(x =>
            string.IsNullOrEmpty(orderParams.Search) 
                || x.Id.ToString().Contains(orderParams.Search) 
                || x.Customer.PhoneNumber.ToString().Contains(orderParams.Search)
        )
        {
        }
    }
}