using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;
        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<List<OfflineOrder>> GetOrdersWithSpec(ISpecification<OfflineOrder> spec)
        {
            var orders = await SpecificationEvaluator<OfflineOrder>.GetQuery(_context.Set<OfflineOrder>().AsQueryable(), spec)
                .Include(o => o.OfflineOrderSKUs)
                .ThenInclude(oos => oos.ProductSKU)
                .ThenInclude(s => s.Product)
                .Include(o => o.OfflineOrderSKUs)
                .ThenInclude(oos => oos.ProductSKU)
                .ThenInclude(s => s.ProductSKUValues)
                .ThenInclude(sv => sv.ProductOptionValue)
                .ThenInclude(ov => ov.ProductOption)
                .ToListAsync();

            return orders;
        }
    }
}