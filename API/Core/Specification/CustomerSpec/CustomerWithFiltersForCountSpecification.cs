using Core.Entities;
using Core.Specification.CustomerSpecification;

namespace Core.Specification.CustomerSpec
{
    public class CustomerWithFiltersForCountSpecification : BaseSpecification<Customer>
    {
        public CustomerWithFiltersForCountSpecification(CustomerSpecParams customerParams) : base(x =>
            string.IsNullOrEmpty(customerParams.Search) 
                || x.Id.ToString().Contains(customerParams.Search) 
                || x.PhoneNumber.ToString().Contains(customerParams.Search)
        )
        {
        }
    }
}