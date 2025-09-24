// using System.Collections;
// using System.Reflection;
// using Core.Entities;
// using Core.Entities.RecommendedData;
// using Core.Enums;
// using Core.Interfaces;
// using Core.Settings;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;

// namespace Core.Services
// {
//     public class RecommendationService : IRecommendationService
//     {
//         private readonly IRecommendationRepository _repository;
//         private readonly IAppUserRepository _appUserRepository;
//         private readonly IUserSyncService _userSyncService;
//         private readonly ICollaborativeFilteringService _collaborativeFiltering;
//         private readonly IContentBasedFilteringService _contentBasedFiltering;
//         private readonly IMemoryCache _cache;
//         private readonly ILogger<RecommendationService> _logger;
//         private readonly RecommendationSettings _settings;

//         public RecommendationService(
//         IRecommendationRepository repository,
//         IAppUserRepository appUserRepository,
//         IUserSyncService userSyncService,
//         ICollaborativeFilteringService collaborativeFiltering,
//         IContentBasedFilteringService contentBasedFiltering,
//         IMemoryCache cache,
//         ILogger<RecommendationService> logger,
//         IConfiguration configuration)
//         {
//             _repository = repository;
//             _appUserRepository = appUserRepository;
//             _userSyncService = userSyncService;
//             _collaborativeFiltering = collaborativeFiltering;
//             _contentBasedFiltering = contentBasedFiltering;
//             _cache = cache;
//             _logger = logger;
//             _settings = configuration.GetSection("RecommendationSettings").Get<RecommendationSettings>() ?? new();
//         }

//         public async Task<List<Recommendation>> GetPersonalizedRecommendationsAsync(int identityUserId, int count = 10)
//         {
//             var cacheKey = $"recommendations_user_{identityUserId}_{count}";

//             // Check cache first
//             if (_cache.TryGetValue(cacheKey, out List<Recommendation>? cachedRecs) && cachedRecs != null)
//             {
//                 return cachedRecs;
//             }

//             try
//             {
//                 // Ensure app user exists
//                 var appUser = await EnsureAppUserExistsAsync(identityUserId);
//                 if (appUser == null)
//                 {
//                     return await GetFallbackRecommendations(identityUserId, count);
//                 }

//                 // Update last active time
//                 await _userSyncService.UpdateAppUserLastActiveAsync(appUser.Id);

//                 // Check if user has enough interactions for collaborative filtering
//                 var userInteractions = await _repository.GetUserInteractionsAsync(appUser.Id);
//                 var useCollaborativeFiltering = userInteractions.Count >= _settings.MinInteractionsForCollaborativeFiltering;

//                 List<Recommendation> recommendations;

//                 if (useCollaborativeFiltering)
//                 {
//                     // Hybrid approach for users with enough data
//                     var collaborativeRecs = await _collaborativeFiltering.GetRecommendationsAsync(appUser.Id, count);
//                     var contentBasedRecs = await _contentBasedFiltering.GetRecommendationsAsync(appUser.Id, count);
//                     recommendations = CombineRecommendations(collaborativeRecs, contentBasedRecs, count);
//                 }
//                 else
//                 {
//                     // Content-based only for new users
//                     var contentBasedRecs = await _contentBasedFiltering.GetRecommendationsAsync(appUser.Id, count);
//                     var popularRecs = await GetPopularRecommendations(identityUserId, count / 2);
//                     recommendations = CombineContentAndPopular(contentBasedRecs, popularRecs, count);
//                 }

//                 // Apply business rules and filters
//                 recommendations = await ApplyBusinessFilters(recommendations, appUser);

//                 // Save and cache recommendations
//                 await _repository.SaveRecommendationsAsync(recommendations);
//                 _cache.Set(cacheKey, recommendations, TimeSpan.FromHours(_settings.RecommendationCacheExpirationHours));

//                 return recommendations;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating personalized recommendations for user {UserId}", identityUserId);
//                 return await GetFallbackRecommendations(identityUserId, count);
//             }
//         }

//         public async Task<List<Product>> GetSimilarProductsAsync(int productId, int count = 5)
//         {
//             var cacheKey = $"similar_products_{productId}_{count}";

//             if (_cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts != null)
//             {
//                 return cachedProducts;
//             }

//             try
//             {
//                 var similarProducts = await _contentBasedFiltering.GetSimilarProductsAsync(productId, count);
//                 _cache.Set(cacheKey, similarProducts, TimeSpan.FromHours(2));
//                 return similarProducts;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error getting similar products for product {ProductId}", productId);
//                 return new List<Product>();
//             }
//         }

//         public async Task<List<Product>> GetPopularProductsAsync(int count = 10)
//         {
//             var cacheKey = $"popular_products_{count}";

//             if (_cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts != null)
//             {
//                 return cachedProducts;
//             }

//             try
//             {
//                 var interactions = await _repository.GetAllInteractionsAsync();
//                 var currentDate = DateTime.UtcNow;

//                 var popularProducts = interactions
//                     .GroupBy(i => i.ProductId)
//                     .Select(g => new
//                     {
//                         ProductId = g.Key,
//                         Score = CalculatePopularityScore(g.ToList(), currentDate)
//                     })
//                     .Where(x => x.Score > 0)
//                     .OrderByDescending(x => x.Score)
//                     .Take(count)
//                     .Select(x => x.ProductId)
//                     .ToList();

//                 var products = await _repository.GetProductsByIdsAsync(popularProducts);
//                 _cache.Set(cacheKey, products, TimeSpan.FromHours(1));

//                 return products;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error getting popular products");
//                 return new List<Product>();
//             }
//         }

//         public async Task<List<Product>> GetTrendingProductsAsync(int count = 10)
//         {
//             var cacheKey = $"trending_products_{count}";

//             if (_cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts != null)
//             {
//                 return cachedProducts;
//             }

//             try
//             {
//                 var recentInteractions = await GetRecentInteractions(TimeSpan.FromDays(7));

//                 var trendingProducts = recentInteractions
//                     .GroupBy(i => i.ProductId)
//                     .Select(g => new
//                     {
//                         ProductId = g.Key,
//                         TrendScore = CalculateTrendScore(g.ToList())
//                     })
//                     .OrderByDescending(x => x.TrendScore)
//                     .Take(count)
//                     .Select(x => x.ProductId)
//                     .ToList();

//                 var products = await _repository.GetProductsByIdsAsync(trendingProducts);
//                 _cache.Set(cacheKey, products, TimeSpan.FromMinutes(30));

//                 return products;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error getting trending products");
//                 return new List<Product>();
//             }
//         }

//         public async Task TrainModelAsync()
//         {
//             _logger.LogInformation("Starting recommendation model training...");

//             try
//             {
//                 // Train collaborative filtering model
//                 await _collaborativeFiltering.TrainModelAsync();

//                 // Update content-based features
//                 await _contentBasedFiltering.UpdateProductFeaturesAsync();

//                 // Clear recommendation caches
//                 ClearRecommendationCaches();

//                 _logger.LogInformation("Model training completed successfully");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error during model training");
//                 throw;
//             }
//         }

//         public async Task<double> CalculateUserProductAffinityAsync(int identityUserId, int productId)
//         {
//             try
//             {
//                 var appUser = await EnsureAppUserExistsAsync(identityUserId);
//                 if (appUser == null) return 0.0;

//                 var collaborativeScore = await _collaborativeFiltering.PredictRatingAsync(appUser.Id, productId);
//                 var contentBasedScore = await _contentBasedFiltering.CalculateSimilarityAsync(appUser.Id, productId);

//                 // Weighted combination based on user's interaction history
//                 var userInteractions = await _repository.GetUserInteractionsAsync(appUser.Id);
//                 var collaborativeWeight = userInteractions.Count >= _settings.MinInteractionsForCollaborativeFiltering ? 0.7 : 0.3;
//                 var contentWeight = 1.0 - collaborativeWeight;

//                 return (collaborativeScore * collaborativeWeight) + (contentBasedScore * contentWeight);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error calculating user-product affinity for user {UserId}, product {ProductId}", identityUserId, productId);
//                 return 0.0;
//             }
//         }

//         // Private helper methods
//         private async Task<User?> EnsureAppUserExistsAsync(int identityUserId)
//         {
//             var appUser = await _appUserRepository.GetByIdAsync(identityUserId);

//             if (appUser == null)
//             {
//                 _logger.LogWarning("App user {UserId} not found, creating basic user", identityUserId);
//                 await _userSyncService.CreateAppUserAsync(identityUserId, "User");
//                 appUser = await _appUserRepository.GetByIdAsync(identityUserId);
//             }

//             return appUser;
//         }

//         private List<Recommendation> CombineRecommendations(
//         List<Recommendation> collaborative,
//         List<Recommendation> contentBased,
//         int count)
//         {
//             var combined = new Dictionary<int, Recommendation>();

//             // Add collaborative filtering recommendations with higher weight
//             foreach (var rec in collaborative)
//             {
//                 combined[rec.ProductId] = new Recommendation
//                 {
//                     UserId = rec.UserId,
//                     ProductId = rec.ProductId,
//                     Product = rec.Product,
//                     Score = rec.Score * 0.7,
//                     Type = RecommendationType.Collaborative,
//                     Reason = "Based on users with similar preferences",
//                     GeneratedAt = DateTime.UtcNow
//                 };
//             }

//             // Add content-based recommendations
//             foreach (var rec in contentBased.Where(cb => !combined.ContainsKey(cb.ProductId)))
//             {
//                 combined[rec.ProductId] = new Recommendation
//                 {
//                     UserId = rec.UserId,
//                     ProductId = rec.ProductId,
//                     Product = rec.Product,
//                     Score = rec.Score * 0.3,
//                     Type = RecommendationType.ContentBased,
//                     Reason = "Based on your product preferences",
//                     GeneratedAt = DateTime.UtcNow
//                 };
//             }

//             // Boost scores for products that appear in both
//             foreach (var contentRec in contentBased.Where(cb => combined.ContainsKey(cb.ProductId)))
//             {
//                 combined[contentRec.ProductId].Score += contentRec.Score * 0.3;
//                 combined[contentRec.ProductId].Type = RecommendationType.Collaborative;
//                 combined[contentRec.ProductId].Reason = "Based on similar users and your preferences";
//             }

//             return combined.Values
//                 .OrderByDescending(r => r.Score)
//                 .Take(count)
//                 .ToList();
//         }

//         private List<Recommendation> CombineContentAndPopular(
//         List<Recommendation> contentBased,
//         List<Recommendation> popular,
//         int count)
//         {
//             var combined = new Dictionary<int, Recommendation>();

//             // Add content-based with higher weight
//             foreach (var rec in contentBased)
//             {
//                 combined[rec.ProductId] = rec;
//             }

//             // Fill remaining slots with popular items
//             var remainingSlots = count - combined.Count;
//             foreach (var rec in popular.Take(remainingSlots).Where(p => !combined.ContainsKey(p.ProductId)))
//             {
//                 combined[rec.ProductId] = rec;
//             }

//             return combined.Values
//                 .OrderByDescending(r => r.Score)
//                 .Take(count)
//                 .ToList();
//         }

//         private async Task<List<Recommendation>> GetFallbackRecommendations(int identityUserId, int count)
//         {
//             var popularProducts = await GetPopularProductsAsync(count);

//             return popularProducts.Select(p => new Recommendation
//             {
//                 UserId = identityUserId,
//                 ProductId = p.Id,
//                 Product = p,
//                 Score = 0.5,
//                 Type = RecommendationType.Popular,
//                 Reason = "Popular products",
//                 GeneratedAt = DateTime.UtcNow
//             }).ToList();
//         }

//         private async Task<List<Recommendation>> GetPopularRecommendations(int identityUserId, int count)
//         {
//             var popularProducts = await GetPopularProductsAsync(count);

//             return popularProducts.Select(p => new Recommendation
//             {
//                 UserId = identityUserId,
//                 ProductId = p.Id,
//                 Product = p,
//                 Score = 0.4,
//                 Type = RecommendationType.Popular,
//                 Reason = "Popular products",
//                 GeneratedAt = DateTime.UtcNow
//             }).ToList();
//         }

//         private async Task<List<UserInteraction>> GetRecentInteractions(TimeSpan timeSpan)
//         {
//             var interactions = await _repository.GetAllInteractionsAsync();
//             var cutoffDate = DateTime.UtcNow.Subtract(timeSpan);

//             return interactions.Where(i => i.Timestamp >= cutoffDate).ToList();
//         }

//         private double CalculatePopularityScore(List<UserInteraction> interactions, DateTime currentDate)
//         {
//             if (!interactions.Any()) return 0.0;

//             var totalScore = 0.0;

//             foreach (var interaction in interactions)
//             {
//                 // Time decay factor
//                 var daysDiff = (currentDate - interaction.Timestamp).TotalDays;
//                 var timeDecay = Math.Exp(-daysDiff / _settings.TimeDecaySettings.HalfLifeDays);

//                 // Interaction weight
//                 var actionWeight = interaction.Type switch
//                 {
//                     InteractionType.View => _settings.WeightSettings.ViewWeight,
//                     InteractionType.Like => _settings.WeightSettings.LikeWeight,
//                     InteractionType.AddToCart => _settings.WeightSettings.AddToCartWeight,
//                     InteractionType.Purchase => _settings.WeightSettings.PurchaseWeight,
//                     InteractionType.Rate => _settings.WeightSettings.RatingWeight * (interaction.Rating / 5.0),
//                     _ => 1.0
//                 };

//                 totalScore += timeDecay * actionWeight;
//             }

//             return totalScore;
//         }

//         private double CalculateTrendScore(List<UserInteraction> interactions)
//         {
//             var now = DateTime.UtcNow;
//             var totalScore = 0.0;

//             foreach (var interaction in interactions)
//             {
//                 var daysDiff = (now - interaction.Timestamp).TotalDays;
//                 var timeWeight = Math.Exp(-daysDiff / 3.0); // More aggressive decay for trending

//                 var actionWeight = interaction.Type switch
//                 {
//                     InteractionType.View => 1.0,
//                     InteractionType.Like => 2.0,
//                     InteractionType.AddToCart => 3.0,
//                     InteractionType.Purchase => 5.0,
//                     InteractionType.Rate => interaction.Rating,
//                     _ => 1.0
//                 };

//                 totalScore += timeWeight * actionWeight;
//             }

//             return totalScore;
//         }

//         private async Task<List<Recommendation>> ApplyBusinessFilters(List<Recommendation> recommendations, User appUser)
//         {
//             // Get user profile for filtering
//             var userProfile = await _appUserRepository.GetByIdWithProfileAsync(appUser.Id);
//             if (userProfile?.Profile == null)
//             {
//                 return recommendations;
//             }

//             // Apply price filters
//             var filteredRecommendations = recommendations
//                 .Where(r => r.Product.ProductSKUs.First().Price >= userProfile.Profile.MinPrice &&
//                            r.Product.ProductSKUs.First().Price <= userProfile.Profile.MaxPrice)
//                 .ToList();

//             // Apply category preferences if any
//             if (userProfile.Profile.PreferredCategories.Any())
//             {
//                 var categoryFiltered = filteredRecommendations
//                     .Where(r => userProfile.Profile.PreferredCategories.Contains(r.Product.ProductTypeId))
//                     .ToList();

//                 // If category filtering removes too many items, keep original list
//                 if (categoryFiltered.Count >= recommendations.Count / 2)
//                 {
//                     filteredRecommendations = categoryFiltered;
//                 }
//             }

//             return filteredRecommendations.Any() ? filteredRecommendations : recommendations;
//         }

//         private void ClearRecommendationCaches()
//         {
//             // This is a simplified cache clearing - in production you might want more sophisticated cache management
//             if (_cache is MemoryCache memoryCache)
//             {
//                 var field = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
//                 if (field?.GetValue(memoryCache) is object coherentState)
//                 {
//                     var entriesCollection = coherentState.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
//                     var entries = (IDictionary?)entriesCollection?.GetValue(coherentState);
//                     entries?.Clear();
//                 }
//             }
//         }
//     }
// }