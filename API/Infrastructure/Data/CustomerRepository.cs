using Core.Interfaces;

namespace Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StoreContext _context;
        public CustomerRepository(StoreContext context)
        {
            _context = context;
        }

        
    }
}