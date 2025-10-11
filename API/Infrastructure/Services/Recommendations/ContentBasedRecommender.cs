using Core.DTOs.Recommendations;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Recommendations;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Recommendations
{
    public class ContentBasedRecommender : IContentBasedRecommender
    {
        private readonly ISessionInteractionRepository _interactionRepo;
        private readonly IRecommendationRepository _recommendationRepo;
        private readonly IProductOptionClassifier _classifier;
        private readonly StoreContext _context;

        public ContentBasedRecommender(
            ISessionInteractionRepository interactionRepo,
            IRecommendationRepository recommendationRepo,
            IProductOptionClassifier classifier,
            StoreContext context)
        {
            _interactionRepo = interactionRepo;
            _recommendationRepo = recommendationRepo;
            _classifier = classifier;
            _context = context;
        }

        public async Task<List<RecommendationDTO>> GetRecommendationsAsync(string sessionId, int limit = 10)
        {
            var viewedProductIds = await _recommendationRepo.GetSessionViewedProductsAsync(sessionId, 20);

            if (!viewedProductIds.Any())
                return new List<RecommendationDTO>();

            var viewedProducts = await _context.Products
                .Where(p => viewedProductIds.Contains(p.Id))
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .ToListAsync();

            var preferences = ExtractSessionPreferences(viewedProducts);

            var candidateProducts = await _context.Products
                .Where(p => !viewedProductIds.Contains(p.Id) && !p.IsDeleted)
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.Photos)
                .ToListAsync();

            candidateProducts = candidateProducts.Where(p => p.HasStock()).ToList();

            var recommendations = new List<(Product product, double score)>();

            foreach (var product in candidateProducts)
            {
                var similarity = CalculateSimilarity(preferences, product);
                if (similarity > 0.3)
                {
                    recommendations.Add((product, similarity));
                }
            }

            return recommendations
                .OrderByDescending(r => r.score)
                .Take(limit)
                .Select(r => ToRecommendationDto(r.product, r.score, "similar_style"))
                .ToList();
        }

        public async Task<List<RecommendationDTO>> GetSimilarProductsAsync(int productId, int limit = 8)
        {
            var sourceProduct = await _context.Products
                .Where(p => p.Id == productId)
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .FirstOrDefaultAsync();

            if (sourceProduct == null)
                return new List<RecommendationDTO>();

            var preferences = ExtractSessionPreferences(new List<Product> { sourceProduct });

            var candidateProducts = await _context.Products
                .Where(p => p.Id != productId && !p.IsDeleted)
                .Include(p => p.ProductSKUs)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.Photos)
                .ToListAsync();

            candidateProducts = candidateProducts.Where(p => p.HasStock()).ToList();

            var recommendations = candidateProducts
                .Select(p => (product: p, score: CalculateSimilarity(preferences, p)))
                .Where(r => r.score >= 0.2)
                .OrderByDescending(r => r.score)
                .Take(limit)
                .Select(r => ToRecommendationDto(r.product, r.score, "similar_style"))
                .ToList();

            return recommendations;
        }

        private SessionPreferences ExtractSessionPreferences(List<Product> viewedProducts)
        {
            var preferences = new SessionPreferences();

            foreach (var product in viewedProducts)
            {
                preferences.ProductTypeFrequency.AddOrIncrement(product.ProductTypeId);

                if (!string.IsNullOrEmpty(product.Style))
                    preferences.StyleFrequency.AddOrIncrement(product.Style);
                if (!string.IsNullOrEmpty(product.Season))
                    preferences.SeasonFrequency.AddOrIncrement(product.Season);
                if (!string.IsNullOrEmpty(product.Material))
                    preferences.MaterialFrequency.AddOrIncrement(product.Material);

                var colors = product.GetColors();
                foreach (var color in colors)
                    preferences.ColorFrequency.AddOrIncrement(color);

                var materials = product.GetMaterials();
                foreach (var material in materials)
                    preferences.MaterialFrequency.AddOrIncrement(material);

                preferences.PriceSum += product.GetRepresentativePrice();
            }

            preferences.AveragePrice = preferences.PriceSum / viewedProducts.Count;
            preferences.Normalize();

            return preferences;
        }

        private double CalculateSimilarity(SessionPreferences preferences, Product product)
        {
            double score = 0;

            if (preferences.ProductTypeFrequency.ContainsKey(product.ProductTypeId))
                score += 0.25 * preferences.ProductTypeFrequency[product.ProductTypeId];

            if (!string.IsNullOrEmpty(product.Style) && preferences.StyleFrequency.ContainsKey(product.Style))
                score += 0.15 * preferences.StyleFrequency[product.Style];

            var productColors = product.GetColors();
            if (productColors.Any())
            {
                var colorScore = productColors.Average(c => preferences.ColorFrequency.GetValueOrDefault(c, 0));
                score += 0.15 * colorScore;
            }

            var productMaterials = product.GetMaterials();
            if (!string.IsNullOrEmpty(product.Material))
                productMaterials.Add(product.Material);

            if (productMaterials.Any())
            {
                var materialScore = productMaterials.Average(m => preferences.MaterialFrequency.GetValueOrDefault(m, 0));
                score += 0.15 * materialScore;
            }

            if (!string.IsNullOrEmpty(product.Season) && preferences.SeasonFrequency.ContainsKey(product.Season))
                score += 0.05 * preferences.SeasonFrequency[product.Season];

            var productPrice = product.GetRepresentativePrice();
            var priceDiff = Math.Abs(productPrice - preferences.AveragePrice);
            var priceRatio = 1 - Math.Min(1, (double)priceDiff / (double)preferences.AveragePrice);
            score += 0.15 * priceRatio;

            return score;
        }

        private RecommendationDTO ToRecommendationDto(Product product, double score, string reasonCode)
        {
            var priceRange = product.GetPriceRange();

            return new RecommendationDTO
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                MinPrice = priceRange.minPrice,
                MaxPrice = priceRange.maxPrice,
                PriceDisplay = FormatPriceDisplay(priceRange.minPrice, priceRange.maxPrice),
                ImageUrl = product.Photos?.FirstOrDefault()?.Url,
                TotalStock = product.TotalStock(),
                AvailableColors = product.GetColors(),
                AvailableSizes = product.GetAvailableSizes(),
                Score = score,
                ReasonCode = reasonCode,
                ReasonText = GetReasonText(reasonCode)
            };
        }

        private string FormatPriceDisplay(decimal minPrice, decimal maxPrice)
        {
            if (minPrice == maxPrice)
                return $"{minPrice:N0}đ";
            return $"{minPrice:N0}đ - {maxPrice:N0}đ";
        }

        private string GetReasonText(string reasonCode)
        {
            return reasonCode switch
            {
                "viewed_together" => "Khách hàng cũng xem",
                "similar_style" => "Phong cách tương tự",
                "trending" => "Đang thịnh hành",
                "best_seller" => "Bán chạy nhất",
                _ => "Gợi ý cho bạn"
            };
        }
    }

    // Helper class
    public class SessionPreferences
    {
        public Dictionary<int, double> ProductTypeFrequency { get; set; } = new();
        public Dictionary<string, double> StyleFrequency { get; set; } = new();
        public Dictionary<string, double> ColorFrequency { get; set; } = new();
        public Dictionary<string, double> MaterialFrequency { get; set; } = new();
        public Dictionary<string, double> SeasonFrequency { get; set; } = new();
        public decimal AveragePrice { get; set; }
        public decimal PriceSum { get; set; }

        public void Normalize()
        {
            NormalizeDictionary(ProductTypeFrequency);
            NormalizeDictionary(StyleFrequency);
            NormalizeDictionary(ColorFrequency);
            NormalizeDictionary(MaterialFrequency);
            NormalizeDictionary(SeasonFrequency);
        }

        private void NormalizeDictionary<T>(Dictionary<T, double> dict)
        {
            var sum = dict.Values.Sum();
            if (sum > 0)
            {
                var keys = dict.Keys.ToList();
                foreach (var key in keys)
                {
                    dict[key] /= sum;
                }
            }
        }
    }
}