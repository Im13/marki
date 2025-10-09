using Core.DTOs.Recommendations;
using Core.Interfaces.Recommendations;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Extensions;

namespace Infrastructure.Services.Recommendations
{
    public class SessionBasedRecommender : ISessionBasedRecommender
    {
        private readonly IRecommendationRepository _recommendationRepo;
        private readonly IProductCoOccurrenceRepository _coOccurrenceRepo;
        private readonly StoreContext _context;

        public SessionBasedRecommender(
            IRecommendationRepository recommendationRepo,
            IProductCoOccurrenceRepository coOccurrenceRepo,
            StoreContext context)
        {
            _recommendationRepo = recommendationRepo;
            _coOccurrenceRepo = coOccurrenceRepo;
            _context = context;
        }

        public async Task<List<RecommendationDTO>> GetRecommendationsAsync(string sessionId, int limit = 10)
        {
            var viewedProducts = await _recommendationRepo.GetSessionViewedProductsAsync(sessionId);
            var cartProducts = await _recommendationRepo.GetSessionCartProductsAsync(sessionId);

            if (!viewedProducts.Any() && !cartProducts.Any())
                return new List<RecommendationDTO>();

            var allInteractedProducts = viewedProducts.Concat(cartProducts).Distinct().ToList();
            var recommendations = new Dictionary<int, double>();

            foreach (var productId in allInteractedProducts)
            {
                var coOccurrences = await _coOccurrenceRepo.GetCoOccurrenceDictionaryAsync(productId, 10);

                foreach (var coOccurrence in coOccurrences)
                {
                    var score = (double)coOccurrence.Value / 100.0;

                    if (recommendations.ContainsKey(coOccurrence.Key))
                        recommendations[coOccurrence.Key] += score;
                    else
                        recommendations[coOccurrence.Key] = score;
                }
            }

            var excludeIds = allInteractedProducts.ToHashSet();
            var filteredRecs = recommendations
                .Where(kvp => !excludeIds.Contains(kvp.Key))
                .OrderByDescending(kvp => kvp.Value)
                .Take(limit)
                .ToList();

            return await ConvertToRecommendationDtos(filteredRecs, "viewed_together");
        }

        private async Task<List<RecommendationDTO>> ConvertToRecommendationDtos(
            List<KeyValuePair<int, double>> recommendations, 
            string reasonCode)
        {
            var productIds = recommendations.Select(r => r.Key).ToList();
            
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id) && !p.IsDeleted)
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.Photos)
                .ToListAsync();

            var result = new List<RecommendationDTO>();

            foreach (var rec in recommendations)
            {
                var product = products.FirstOrDefault(p => p.Id == rec.Key);
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
                    Score = rec.Value,
                    ReasonCode = reasonCode,
                    ReasonText = "Khách hàng cũng xem"
                });
            }

            return result;
        }
    }
}