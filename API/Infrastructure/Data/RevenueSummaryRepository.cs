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

        public async Task DecreaseRevenueAsync(Order order)
        {
            var revenueDate = order.OrderDate.Date;
            var revenue = await GetByDateAsync(revenueDate);

            if (revenue != null)
            {
                revenue.TotalRevenue -= order.Total;
                revenue.TotalOrders = Math.Max(0, revenue.TotalOrders - 1);

                switch (order.Source)
                {
                    case OrderSources.Shopee:
                        revenue.ShopeeRevenue -= order.Total;
                        break;
                    case OrderSources.Facebook:
                        revenue.FacebookRevenue -= order.Total;
                        break;
                    case OrderSources.Instagram:
                        revenue.InstagramRevenue -= order.Total;
                        break;
                    case OrderSources.Website:
                        revenue.WebsiteRevenue -= order.Total;
                        break;
                    case OrderSources.Offline:
                        revenue.OfflineRevenue -= order.Total;
                        break;
                }

                await UpdateAsync(revenue);
            }
        }

        public async Task UpdateDailyRevenueAsync(Order order)
        {
            var cancelled = (int)OrderStatus.Cancelled;
            var deleted = (int)OrderStatus.Deleted;
            var revenueSummary = await _context.RevenueSummaries
                .FirstOrDefaultAsync(r => r.Date.Date == order.OrderDate.Date);

            if (revenueSummary == null)
            {
                // Nếu chưa có RevenueSummary cho ngày này, tạo mới
                revenueSummary = new RevenueSummary
                {
                    Date = order.OrderDate.Date,
                    TotalRevenue = 0,
                    TotalOrders = 0,
                    ShopeeRevenue = 0,
                    FacebookRevenue = 0,
                    InstagramRevenue = 0,
                    WebsiteRevenue = 0,
                    OfflineRevenue = 0
                };
                _context.RevenueSummaries.Add(revenueSummary);
            }

            // Cập nhật doanh thu và số lượng đơn hàng
            revenueSummary.TotalRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);

            revenueSummary.TotalOrders = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .CountAsync();

            // Cập nhật doanh thu theo từng nguồn đơn hàng
            revenueSummary.ShopeeRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.Source == OrderSources.Shopee
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);

            revenueSummary.FacebookRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.Source == OrderSources.Facebook
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);

            revenueSummary.InstagramRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.Source == OrderSources.Instagram
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);

            revenueSummary.WebsiteRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.Source == OrderSources.Website
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);

            revenueSummary.OfflineRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == order.OrderDate.Date
                    && o.Source == OrderSources.Offline
                    && o.OrderStatus.Id != cancelled
                    && o.OrderStatus.Id != deleted)
                .SumAsync(o => o.Total);
                
            await _context.SaveChangesAsync();
        }
    }
}