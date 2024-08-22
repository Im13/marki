using Core.Entities;
using Core.Specification.CustomerSpecification;

namespace Core.Specification.CustomerSpec
{
    public class CustomerSpecification : BaseSpecification<Customer>
    {
        public CustomerSpecification(CustomerSpecParams customerParams) : base(x => 
            string.IsNullOrEmpty(customerParams.Search) 
                || x.Id.ToString().Contains(customerParams.Search) 
                || x.PhoneNumber.ToString().Contains(customerParams.Search)
        )
        {
            AddOrderByDescending(x => x.Id);
            ApplyPaging(customerParams.PageSize * (customerParams.PageIndex - 1), customerParams.PageSize);
        }
    }
}