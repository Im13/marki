using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class RevenueSummaryRepository : GenericRepository<RevenueSummary>, IRevenueSummaryRepository
    {
        private readonly StoreContext _context;
        public RevenueSummaryRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RevenueSummary> GetByDateAsync(DateTime date)
        {
            return await _context.RevenueSummaries.FirstOrDefaultAsync(r => r.Date.Date == date.Date);

        }

        public async Task<IReadOnlyList<RevenueSummary>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.RevenueSummaries
                .Where(r => r.Date.Date >= startDate.Date && r.Date.Date <= endDate.Date)
                .OrderBy(r => r.Date)
                .ToListAsync();

        }

        public async Task UpdateRevenueAsync(Order order)
        {
            var revenueDate = order.OrderDate.Date;
            var revenue = await GetByDateAsync(revenueDate);

            if (revenue == null)
            {
                revenue = new RevenueSummary
                {
                    Date = revenueDate,
                    TotalRevenue = order.Total,
                    TotalOrders = 1,
                    ShopeeRevenue = order.Source == OrderSources.Shopee ? order.Total : 0,
                    FacebookRevenue = order.Source == OrderSources.Facebook ? order.Total : 0,
                    InstagramRevenue = order.Source == OrderSources.Instagram ? order.Total : 0,
                    WebsiteRevenue = order.Source == OrderSources.Website ? order.Total : 0,
                    OfflineRevenue = order.Source == OrderSources.Offline ? order.Total : 0
                };
                await AddAsync(revenue);
            }
            else
            {
                revenue.TotalRevenue += order.Total;
                revenue.TotalOrders += 1;
                
                switch (order.Source)
                {
                    case OrderSources.Shopee:
                        revenue.ShopeeRevenue += order.Total;
                        break;
                    case OrderSources.Facebook:
                        revenue.FacebookRevenue += order.Total;
                        break;
                    case OrderSources.Instagram:
                        revenue.InstagramRevenue += order.Total;
                        break;
                    case OrderSources.Website:
                        revenue.WebsiteRevenue += order.Total;
                        break;
                    case OrderSources.Offline:
                        revenue.OfflineRevenue += order.Total;
                        break;
                }
                
                await UpdateAsync(revenue);
            }
        }
    }
}