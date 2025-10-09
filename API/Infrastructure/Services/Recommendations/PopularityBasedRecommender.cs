using Core.DTOs.Recommendations;
using Core.Enums;
using Core.Interfaces.Recommendations;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Extensions;

namespace Infrastructure.Services.Recommendations
{
    public class PopularityBasedRecommender : IPopularityBasedRecommender
    {
        private readonly IProductTrendingRepository _trendingRepo;
        private readonly StoreContext _context;

        public PopularityBasedRecommender(
            IProductTrendingRepository trendingRepo,
            StoreContext context)
        {
            _trendingRepo = trendingRepo;
            _context = context;
        }

        public async Task<List<RecommendationDTO>> GetTrendingProductsAsync(int limit = 10)
        {
            var trendingProducts = await _trendingRepo.GetTopTrendingAsync(7, limit);

            var result = new List<RecommendationDTO>();

            foreach (var trending in trendingProducts)
            {
                if (trending.Product == null || !trending.Product.HasStock())
                    continue;

                var priceRange = trending.Product.GetPriceRange();

                result.Add(new RecommendationDTO
                {
                    ProductId = trending.Product.Id,
                    ProductName = trending.Product.Name,
                    ProductSlug = trending.Product.Slug,
                    MinPrice = priceRange.minPrice,
                    MaxPrice = priceRange.maxPrice,
                    PriceDisplay = priceRange.minPrice == priceRange.maxPrice 
                        ? $"{priceRange.minPrice:N0}đ" 
                        : $"{priceRange.minPrice:N0}đ - {priceRange.maxPrice:N0}đ",
                    ImageUrl = trending.Product.Photos?.FirstOrDefault()?.Url,
                    TotalStock = trending.Product.TotalStock(),
                    AvailableColors = trending.Product.GetColors(),
                    AvailableSizes = trending.Product.GetAvailableSizes(),
                    Score = (double)trending.TrendingScore,
                    ReasonCode = "trending",
                    ReasonText = "Đang thịnh hành"
                });
            }

            return result;
        }

        public async Task<List<RecommendationDTO>> GetBestSellersAsync(int days = 30, int limit = 10)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);

            var bestSellers = await _context.SessionInteractions
                .Where(i => i.InteractionType == InteractionType.Purchase && i.InteractionDate >= startDate)
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    PurchaseCount = g.Count()
                })
                .OrderByDescending(x => x.PurchaseCount)
                .Take(limit)
                .ToListAsync();

            var productIds = bestSellers.Select(b => b.ProductId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id) && !p.IsDeleted)
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.Photos)
                .ToListAsync();

            var result = new List<RecommendationDTO>();

            foreach (var bestSeller in bestSellers)
            {
                var product = products.FirstOrDefault(p => p.Id == bestSeller.ProductId);
                if (product == null || !product.HasStock())
                    continue;

                var priceRange = product.GetPriceRange();

                result.Add(new RecommendationDTO
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductSlug = product.Slug,
                    MinPrice = priceRange.minPrice,
                    MaxPrice = priceRange.maxPrice,
                    PriceDisplay = priceRange.minPrice == priceRange.maxPrice 
                        ? $"{priceRange.minPrice:N0}đ" 
                        : $"{priceRange.minPrice:N0}đ - {priceRange.maxPrice:N0}đ",
                    ImageUrl = product.Photos?.FirstOrDefault()?.Url,
                    TotalStock = product.TotalStock(),
                    AvailableColors = product.GetColors(),
                    AvailableSizes = product.GetAvailableSizes(),
                    Score = bestSeller.PurchaseCount,
                    ReasonCode = "best_seller",
                    ReasonText = "Bán chạy nhất"
                });
            }

            return result;
        }
    }
}