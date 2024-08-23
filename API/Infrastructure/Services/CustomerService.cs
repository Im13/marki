using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteCustomers(List<int> customerIds)
        {
            foreach(var id in customerIds) 
            {
                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

                customer.IsDeleted = true;

                _unitOfWork.Repository<Customer>().Update(customer);
            }

            var result = await _unitOfWork.Complete();

            if(result <= 0) return false;

            return true;
        }
    }
}