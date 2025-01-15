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
                .Include(o => o.Ward)
                .Include(o => o.District)
                .Include(o => o.Province)
                .Include(o => o.OrderStatus)
                .ToListAsync();

            return orders;
        }

        public async Task<List<Order>> GetWebsiteOrdersWithSpec(ISpecification<Order> spec)
        {
            var orders = await SpecificationEvaluator<Order>.GetQuery(_context.Set<Order>().AsQueryable(), spec)
                .ToListAsync();

            return orders;
        }

        public async Task<Order> GetWebsiteOrderById(int id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}