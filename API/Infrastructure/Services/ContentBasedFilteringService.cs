// using Core.Entities;
// using Core.Entities.RecommendedData;
// using Core.Enums;
// using Core.Interfaces;
// using Core.Settings;
// using MathNet.Numerics.LinearAlgebra;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;

// namespace Infrastructure.Services
// {
//     public class ContentBasedFilteringService : IContentBasedFilteringService
//     {
//         private readonly IRecommendationRepository _repository;
//         private readonly IMemoryCache _cache;
//         private readonly ILogger<ContentBasedFilteringService> _logger;
//         private readonly RecommendationSettings _settings;

//         // Feature state
//         private Dictionary<int, Vector<double>> _productFeatures = new();
//         private Dictionary<int, Vector<double>> _userProfiles = new();
//         private Dictionary<string, int> _categoryToIndex = new();
//         private Dictionary<string, int> _tagToIndex = new();
//         private const int NumFeatures = 100; // Total feature vector size

//         public ContentBasedFilteringService(
//             IRecommendationRepository repository,
//             IMemoryCache cache,
//             ILogger<ContentBasedFilteringService> logger,
//             IConfiguration configuration)
//         {
//             _repository = repository;
//             _cache = cache;
//             _logger = logger;
//             _settings = configuration.GetSection("RecommendationSettings").Get<RecommendationSettings>() ?? new();
//         }

//         public async Task<List<Recommendation>> GetRecommendationsAsync(int appUserId, int count = 10)
//         {
//             if (!_userProfiles.ContainsKey(appUserId))
//             {
//                 await BuildUserProfile(appUserId);
//             }

//             if (!_userProfiles.ContainsKey(appUserId))
//             {
//                 _logger.LogWarning("Could not build user profile for user {UserId}", appUserId);
//                 return new List<Recommendation>();
//             }

//             var recommendations = new List<Recommendation>();

//             try
//             {
//                 var userProfile = _userProfiles[appUserId];

//                 // Get user's interaction history to avoid recommending already interacted items
//                 var userInteractions = await _repository.GetUserInteractionsAsync(appUserId);
//                 var interactedProductIds = userInteractions.Select(i => i.ProductId).ToHashSet();

//                 // Calculate similarity between user profile and all products
//                 var productSimilarities = new List<(int productId, double similarity)>();

//                 await Task.Run(() =>
//                 {
//                     Parallel.ForEach(_productFeatures, kvp =>
//                     {
//                         var (productId, productFeatures) = kvp;
//                         if (interactedProductIds.Contains(productId))
//                             return;

//                         var similarity = CosineSimilarity(userProfile, productFeatures);
//                         if (similarity > _settings.MinSimilarityThreshold)
//                         {
//                             lock (productSimilarities)
//                             {
//                                 productSimilarities.Add((productId, similarity));
//                             }
//                         }
//                     });
//                 });

//                 // Sort by similarity and take top recommendations
//                 var topSimilar = productSimilarities
//                     .OrderByDescending(x => x.similarity)
//                     .Take(count)
//                     .ToList();

//                 foreach (var (productId, similarity) in topSimilar)
//                 {
//                     recommendations.Add(new Recommendation
//                     {
//                         UserId = appUserId,
//                         ProductId = productId,
//                         Score = similarity,
//                         Type = RecommendationType.ContentBased,
//                         Reason = GenerateContentBasedReason(appUserId, productId),
//                         GeneratedAt = DateTime.UtcNow
//                     });
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating content-based recommendations for user {UserId}", appUserId);
//             }

//             return recommendations;
//         }

//         public async Task<List<Product>> GetSimilarProductsAsync(int productId, int count = 5)
//         {
//             var cacheKey = $"similar_products_cb_{productId}_{count}";

//             if (_cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts != null)
//             {
//                 return cachedProducts;
//             }

//             if (!_productFeatures.ContainsKey(productId))
//             {
//                 return new List<Product>();
//             }

//             try
//             {
//                 var targetProductFeatures = _productFeatures[productId];
//                 var similarities = new List<(int productId, double similarity)>();

//                 await Task.Run(() =>
//                 {
//                     Parallel.ForEach(_productFeatures, kvp =>
//                     {
//                         var (otherProductId, otherFeatures) = kvp;
//                         if (otherProductId == productId) return;

//                         var similarity = CosineSimilarity(targetProductFeatures, otherFeatures);
//                         lock (similarities)
//                         {
//                             similarities.Add((otherProductId, similarity));
//                         }
//                     });
//                 });

//                 var similarProductIds = similarities
//                     .OrderByDescending(x => x.similarity)
//                     .Take(count)
//                     .Select(x => x.productId)
//                     .ToList();

//                 var products = await _repository.GetProductsByIdsAsync(similarProductIds);
//                 _cache.Set(cacheKey, products, TimeSpan.FromHours(4));

//                 return products;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error getting similar products for product {ProductId}", productId);
//                 return new List<Product>();
//             }
//         }

//         public async Task<double> CalculateSimilarityAsync(int appUserId, int productId)
//         {
//             if (!_userProfiles.ContainsKey(appUserId))
//             {
//                 await BuildUserProfile(appUserId);
//             }

//             if (!_productFeatures.ContainsKey(productId) || !_userProfiles.ContainsKey(appUserId))
//             {
//                 return 0.0;
//             }

//             return CosineSimilarity(_userProfiles[appUserId], _productFeatures[productId]);
//         }

//         public async Task UpdateProductFeaturesAsync()
//         {
//             _logger.LogInformation("Updating product features for content-based filtering...");

//             var cacheKey = "cb_features_last_updated";
//             if (_cache.TryGetValue(cacheKey, out DateTime lastUpdated))
//             {
//                 if (DateTime.UtcNow - lastUpdated < TimeSpan.FromHours(_settings.ModelTrainingIntervalHours / 2))
//                 {
//                     _logger.LogInformation("Product features were recently updated, skipping...");
//                     return;
//                 }
//             }

//             try
//             {
//                 var interactions = await _repository.GetAllInteractionsAsync();
//                 var allProducts = await _repository.GetAllProductsAsync();

//                 // Build vocabulary
//                 await BuildVocabulary(allProducts);

//                 // Build product features
//                 await BuildProductFeatures(interactions, allProducts);

//                 _cache.Set(cacheKey, DateTime.UtcNow, TimeSpan.FromHours(_settings.ModelTrainingIntervalHours));

//                 _logger.LogInformation("Product features updated. Products: {ProductCount}, Features: {FeatureCount}",
//                     _productFeatures.Count, NumFeatures);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error updating product features");
//                 throw;
//             }
//         }

//         private async Task BuildVocabulary(List<Product> products)
//         {
//             // Build category vocabulary
//             var categories = products.Select(p => p.ProductType.Name).Distinct().ToList();
//             _categoryToIndex = categories.Select((cat, index) => new { cat, index })
//                 .ToDictionary(x => x.cat, x => x.index);

//             // Build tag vocabulary (top N tags)
//             // var allTags = products.SelectMany(p => p.Tags).ToList();
//             // var topTags = allTags.GroupBy(tag => tag)
//             //     .OrderByDescending(g => g.Count())
//             //     .Take(50) // Top 50 tags
//             //     .Select((g, index) => new { tag = g.Key, index })
//             //     .ToDictionary(x => x.tag, x => x.index);

//             // _tagToIndex = topTags;
//         }

//         private async Task BuildProductFeatures(List<UserInteraction> interactions, List<Product> products)
//         {
//             var productInteractionMap = interactions.GroupBy(i => i.ProductId)
//                 .ToDictionary(g => g.Key, g => g.ToList());

//             await Task.Run(() =>
//             {
//                 Parallel.ForEach(products, product =>
//                 {
//                     var features = new double[NumFeatures];
//                     int featureIndex = 0;

//                     // Category features (one-hot encoding)
//                     for (int i = 0; i < _categoryToIndex.Count && featureIndex < NumFeatures - 10; i++)
//                     {
//                         features[featureIndex++] = _categoryToIndex.ContainsKey(product.ProductType.Name) &&
//                                                    _categoryToIndex[product.ProductType.Name] == i ? 1.0 : 0.0;
//                     }

//                     // Tag features (multi-hot encoding)
//                     for (int i = 0; i < Math.Min(_tagToIndex.Count, 30) && featureIndex < NumFeatures - 10; i++)
//                     {
//                         var tag = _tagToIndex.ElementAtOrDefault(i).Key;
//                         features[featureIndex++] = product.Tags.Contains(tag) ? 1.0 : 0.0;
//                     }

//                     // Price features (normalized)
//                     if (featureIndex < NumFeatures - 5)
//                     {
//                         features[featureIndex++] = Math.Log10((double)product.Price + 1) / 10.0; // Log-normalized price
//                     }

//                     // Interaction-based features
//                     if (productInteractionMap.ContainsKey(product.Id) && featureIndex < NumFeatures - 4)
//                     {
//                         var productInteractions = productInteractionMap[product.Id];

//                         // Average rating
//                         var ratings = productInteractions.Where(i => i.Type == InteractionType.Rate && i.Rating > 0).ToList();
//                         features[featureIndex++] = ratings.Any() ? ratings.Average(r => r.Rating) / 5.0 : 0;

//                         // Popularity (total interactions, normalized)
//                         features[featureIndex++] = Math.Log10(productInteractions.Count + 1) / 10.0;

//                         // Purchase ratio
//                         var purchaseCount = productInteractions.Count(i => i.Type == InteractionType.Purchase);
//                         features[featureIndex++] = productInteractions.Count > 0 ? (double)purchaseCount / productInteractions.Count : 0;

//                         // Recency score
//                         var latestInteraction = productInteractions.Max(i => i.Timestamp);
//                         var daysSince = (DateTime.UtcNow - latestInteraction).TotalDays;
//                         features[featureIndex++] = Math.Exp(-daysSince / 30.0); // 30-day half-life
//                     }

//                     // Product metadata features
//                     if (featureIndex < NumFeatures)
//                     {
//                         // Product age (normalized)
//                         var daysSinceCreation = (DateTime.UtcNow - product.CreatedAt).TotalDays;
//                         features[featureIndex++] = Math.Exp(-daysSinceCreation / 365.0); // 1-year half-life
//                     }

//                     // Normalize the feature vector
//                     var featureVector = Vector<double>.Build.DenseOfArray(features);
//                     var normalizedVector = featureVector.L2Norm() > 1e-10 ? featureVector / featureVector.L2Norm() : featureVector;

//                     _productFeatures[product.Id] = normalizedVector;
//                 });
//             });
//         }

//         private async Task BuildUserProfile(int appUserId)
//         {
//             try
//             {
//                 var userInteractions = await _repository.GetUserInteractionsAsync(appUserId);

//                 if (!userInteractions.Any())
//                 {
//                     // Create default profile for new users
//                     _userProfiles[appUserId] = Vector<double>.Build.Dense(NumFeatures, 0.1);
//                     return;
//                 }

//                 // Build user profile as weighted average of interacted products' features
//                 var profileFeatures = new double[NumFeatures];
//                 var totalWeight = 0.0;

//                 foreach (var interaction in userInteractions)
//                 {
//                     if (!_productFeatures.ContainsKey(interaction.ProductId))
//                         continue;

//                     var productFeatures = _productFeatures[interaction.ProductId];

//                     // Weight based on interaction type and recency
//                     var weight = CalculateInteractionWeight(interaction);

//                     // Add weighted features to profile
//                     for (int i = 0; i < Math.Min(productFeatures.Count, NumFeatures); i++)
//                     {
//                         profileFeatures[i] += productFeatures[i] * weight;
//                     }

//                     totalWeight += weight;
//                 }

//                 // Normalize by total weight
//                 if (totalWeight > 0)
//                 {
//                     for (int i = 0; i < profileFeatures.Length; i++)
//                     {
//                         profileFeatures[i] /= totalWeight;
//                     }
//                 }

//                 var userVector = Vector<double>.Build.DenseOfArray(profileFeatures);
//                 _userProfiles[appUserId] = userVector.L2Norm() > 1e-10 ? userVector / userVector.L2Norm() : userVector;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error building user profile for user {UserId}", appUserId);
//                 _userProfiles[appUserId] = Vector<double>.Build.Dense(NumFeatures, 0.1);
//             }
//         }

//         private double CalculateInteractionWeight(UserInteraction interaction)
//         {
//             // Base weight by interaction type
//             var baseWeight = interaction.Type switch
//             {
//                 InteractionType.View => _settings.WeightSettings.ViewWeight,
//                 InteractionType.Like => _settings.WeightSettings.LikeWeight,
//                 InteractionType.AddToCart => _settings.WeightSettings.AddToCartWeight,
//                 InteractionType.Purchase => _settings.WeightSettings.PurchaseWeight,
//                 InteractionType.Rate => _settings.WeightSettings.RatingWeight * (interaction.Rating / 5.0),
//                 _ => 1.0
//             };

//             // Apply time decay
//             var daysSince = (DateTime.UtcNow - interaction.Timestamp).TotalDays;
//             var timeDecay = Math.Exp(-daysSince / _settings.TimeDecaySettings.HalfLifeDays);

//             return baseWeight * timeDecay;
//         }

//         private string GenerateContentBasedReason(int appUserId, int productId)
//         {
//             // This is a simplified version - in practice you'd analyze which features contributed most to the similarity
//             return "Matches your preferences based on categories and product features";
//         }

//         private static double CosineSimilarity(Vector<double> a, Vector<double> b)
//         {
//             if (a.Count != b.Count) return 0;

//             var dotProduct = a.DotProduct(b);
//             var magnitudeA = a.L2Norm();
//             var magnitudeB = b.L2Norm();

//             if (magnitudeA < 1e-10 || magnitudeB < 1e-10)
//                 return 0;

//             return dotProduct / (magnitudeA * magnitudeB);
//         }
//     }
// }