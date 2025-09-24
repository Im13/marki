// using Core.Interfaces;
// using Core.Settings;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Configuration;
// using Core.Entities.RecommendedData;
// using Core.Enums;
// using System.Numerics;
// using MathNet.Numerics.LinearAlgebra;

// namespace Infrastructure.Services
// {
//     public class CollaborativeFilteringService : ICollaborativeFilteringService
//     {
//         private readonly IRecommendationRepository _repository;
//         private readonly IMemoryCache _cache;
//         private readonly ILogger<CollaborativeFilteringService> _logger;
//         private readonly RecommendationSettings _settings;

//         // Model state
//         private Matrix<double>? _userItemMatrix;
//         private Matrix<double>? _userFeatureMatrix;
//         private Matrix<double>? _itemFeatureMatrix;
//         private Dictionary<int, int> _userIndexMap = new();
//         private Dictionary<int, int> _itemIndexMap = new();
//         private Dictionary<int, int> _indexUserMap = new();
//         private Dictionary<int, int> _indexItemMap = new();

//         // Model parameters
//         private const int NumFactors = 50;
//         private const double LearningRate = 0.01;
//         private const double Regularization = 0.01;
//         private const int MaxIterations = 100;

//         public CollaborativeFilteringService(
//         IRecommendationRepository repository,
//         IMemoryCache cache,
//         ILogger<CollaborativeFilteringService> logger,
//         IConfiguration configuration)
//         {
//             _repository = repository;
//             _cache = cache;
//             _logger = logger;
//             _settings = configuration.GetSection("RecommendationSettings").Get<RecommendationSettings>() ?? new();
//         }

//         public async Task<List<Recommendation>> GetRecommendationsAsync(int appUserId, int count = 10)
//         {
//             if (_userItemMatrix == null || !_userIndexMap.ContainsKey(appUserId))
//             {
//                 await TrainModelAsync();
//             }

//             if (!_userIndexMap.ContainsKey(appUserId))
//             {
//                 _logger.LogWarning("User {UserId} not found in collaborative filtering model", appUserId);
//                 return new List<Recommendation>(); // New user, no collaborative recommendations
//             }

//             var recommendations = new List<Recommendation>();
//             var userIndex = _userIndexMap[appUserId];

//             try
//             {
//                 // Matrix Factorization approach
//                 if (_userFeatureMatrix != null && _itemFeatureMatrix != null)
//                 {
//                     var userFeatures = _userFeatureMatrix.Row(userIndex);
//                     var predictions = new List<(int itemId, double score)>();

//                     for (int itemIndex = 0; itemIndex < _itemFeatureMatrix.RowCount; itemIndex++)
//                     {
//                         var itemFeatures = _itemFeatureMatrix.Row(itemIndex);
//                         var prediction = userFeatures.DotProduct(itemFeatures);

//                         // Only recommend items user hasn't interacted with
//                         if (_userItemMatrix.At(userIndex, itemIndex) == 0 && prediction > _settings.MinSimilarityThreshold)
//                         {
//                             var itemId = _indexItemMap[itemIndex];
//                             predictions.Add((itemId, prediction));
//                         }
//                     }

//                     var topPredictions = predictions
//                         .OrderByDescending(p => p.score)
//                         .Take(count)
//                         .ToList();

//                     foreach (var (itemId, score) in topPredictions)
//                     {
//                         recommendations.Add(new Recommendation
//                         {
//                             AppUserId = appUserId,
//                             ProductId = itemId,
//                             Score = Math.Min(1.0, Math.Max(0.0, score)), // Normalize to 0-1
//                             Type = RecommendationType.Collaborative,
//                             Reason = "Users with similar taste also liked this",
//                             GeneratedAt = DateTime.UtcNow
//                         });
//                     }
//                 }
//                 else
//                 {
//                     // Fallback to user-based collaborative filtering
//                     var similarUsers = await FindSimilarUsers(userIndex, 10);
//                     var candidateItems = GetCandidateItems(similarUsers, appUserId);

//                     foreach (var (itemId, score) in candidateItems.Take(count))
//                     {
//                         recommendations.Add(new Recommendation
//                         {
//                             AppUserId = appUserId,
//                             ProductId = itemId,
//                             Score = score,
//                             Type = RecommendationType.Collaborative,
//                             Reason = "Based on users with similar preferences",
//                             GeneratedAt = DateTime.UtcNow
//                         });
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating collaborative recommendations for user {UserId}", appUserId);
//                 return new List<Recommendation>();
//             }

//             return recommendations;
//         }

//         public async Task<double> PredictRatingAsync(int appUserId, int productId)
//         {
//             if (_userItemMatrix == null || !_userIndexMap.ContainsKey(appUserId) || !_itemIndexMap.ContainsKey(productId))
//             {
//                 return 2.5; // Average rating as default
//             }

//             var userIndex = _userIndexMap[appUserId];
//             var itemIndex = _itemIndexMap[productId];

//             // Check if user already rated this item
//             var existingRating = _userItemMatrix.At(userIndex, itemIndex);
//             if (existingRating > 0)
//             {
//                 return existingRating;
//             }

//             try
//             {
//                 // Matrix Factorization prediction
//                 if (_userFeatureMatrix != null && _itemFeatureMatrix != null)
//                 {
//                     var userFeatures = _userFeatureMatrix.Row(userIndex);
//                     var itemFeatures = _itemFeatureMatrix.Row(itemIndex);
//                     var prediction = userFeatures.DotProduct(itemFeatures);
//                     return Math.Max(1.0, Math.Min(5.0, prediction)); // Clamp to 1-5 range
//                 }

//                 // Fallback to user-based collaborative filtering
//                 var similarUsers = await FindSimilarUsers(userIndex, 5);
//                 var prediction = PredictRatingFromSimilarUsers(itemIndex, similarUsers);

//                 return Math.Max(1.0, Math.Min(5.0, prediction));
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error predicting rating for user {UserId}, product {ProductId}", appUserId, productId);
//                 return 2.5;
//             }
//         }

//         public async Task TrainModelAsync()
//         {
//             _logger.LogInformation("Training collaborative filtering model...");

//             var cacheKey = "cf_model_last_trained";
//             if (_cache.TryGetValue(cacheKey, out DateTime lastTrained))
//             {
//                 if (DateTime.UtcNow - lastTrained < TimeSpan.FromHours(_settings.ModelTrainingIntervalHours / 2))
//                 {
//                     _logger.LogInformation("Model was recently trained, skipping...");
//                     return;
//                 }
//             }

//             try
//             {
//                 var interactions = await _repository.GetAllInteractionsAsync();

//                 if (!interactions.Any())
//                 {
//                     _logger.LogWarning("No interactions found for training");
//                     return;
//                 }

//                 // Build user-item matrix
//                 await BuildUserItemMatrix(interactions);

//                 // Train matrix factorization model
//                 await TrainMatrixFactorization();

//                 _cache.Set(cacheKey, DateTime.UtcNow, TimeSpan.FromHours(_settings.ModelTrainingIntervalHours));

//                 _logger.LogInformation("Collaborative filtering model training completed. Users: {UserCount}, Items: {ItemCount}",
//                     _userIndexMap.Count, _itemIndexMap.Count);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error training collaborative filtering model");
//                 throw;
//             }
//         }

//         private async Task BuildUserItemMatrix(List<UserInteraction> interactions)
//         {
//             // Create mappings
//             var users = interactions.Select(i => i.UserId).Distinct().ToList();
//             var items = interactions.Select(i => i.ProductId).Distinct().ToList();

//             _userIndexMap = users.Select((id, index) => new { id, index })
//                 .ToDictionary(x => x.id, x => x.index);
//             _itemIndexMap = items.Select((id, index) => new { id, index })
//                 .ToDictionary(x => x.id, x => x.index);

//             // Reverse mappings
//             _indexUserMap = _userIndexMap.ToDictionary(x => x.Value, x => x.Key);
//             _indexItemMap = _itemIndexMap.ToDictionary(x => x.Value, x => x.Key);

//             // Initialize matrix with zeros
//             _userItemMatrix = Matrix<double>.Build.Dense(users.Count, items.Count, 0.0);

//             // Fill matrix with ratings/implicit feedback
//             foreach (var interaction in interactions)
//             {
//                 var userIndex = _userIndexMap[interaction.UserId];
//                 var itemIndex = _itemIndexMap[interaction.ProductId];

//                 // Convert different interaction types to rating scale
//                 var rating = ConvertInteractionToRating(interaction);

//                 // Take the maximum rating if multiple interactions exist
//                 var currentRating = _userItemMatrix.At(userIndex, itemIndex);
//                 _userItemMatrix[userIndex, itemIndex] = Math.Max(currentRating, rating);
//             }
//         }

//         private async Task TrainMatrixFactorization()
//         {
//             var numUsers = _userItemMatrix.RowCount;
//             var numItems = _userItemMatrix.ColumnCount;

//             // Initialize feature matrices with random values
//             var random = new Random(42); // Fixed seed for reproducibility
//             _userFeatureMatrix = Matrix<double>.Build.Random(numUsers, NumFactors, random.NextDouble);
//             _itemFeatureMatrix = Matrix<double>.Build.Random(numItems, NumFactors, random.NextDouble);

//             // Stochastic Gradient Descent
//             for (int iter = 0; iter < MaxIterations; iter++)
//             {
//                 double totalError = 0;
//                 int numRatings = 0;

//                 for (int userIndex = 0; userIndex < numUsers; userIndex++)
//                 {
//                     for (int itemIndex = 0; itemIndex < numItems; itemIndex++)
//                     {
//                         var rating = _userItemMatrix.At(userIndex, itemIndex);
//                         if (rating > 0) // Only train on observed ratings
//                         {
//                             var userFeatures = _userFeatureMatrix.Row(userIndex);
//                             var itemFeatures = _itemFeatureMatrix.Row(itemIndex);

//                             var prediction = userFeatures.DotProduct(itemFeatures);
//                             var error = rating - prediction;

//                             totalError += error * error;
//                             numRatings++;

//                             // Update features using gradient descent
//                             for (int factor = 0; factor < NumFactors; factor++)
//                             {
//                                 var userFeature = userFeatures[factor];
//                                 var itemFeature = itemFeatures[factor];

//                                 _userFeatureMatrix[userIndex, factor] += LearningRate * (error * itemFeature - Regularization * userFeature);
//                                 _itemFeatureMatrix[itemIndex, factor] += LearningRate * (error * userFeature - Regularization * itemFeature);
//                             }
//                         }
//                     }
//                 }

//                 var rmse = Math.Sqrt(totalError / numRatings);
//                 if (iter % 20 == 0)
//                 {
//                     _logger.LogDebug("Matrix Factorization iteration {Iter}, RMSE: {RMSE:F4}", iter, rmse);
//                 }

//                 // Early stopping if RMSE is low enough
//                 if (rmse < 0.01) break;
//             }
//         }

//         private async Task<List<(int userId, double similarity)>> FindSimilarUsers(int userIndex, int count)
//         {
//             var similarities = new List<(int userId, double similarity)>();
//             var targetUserVector = _userItemMatrix!.Row(userIndex);

//             await Task.Run(() =>
//             {
//                 Parallel.For(0, _userItemMatrix.RowCount, otherUserIndex =>
//                 {
//                     if (otherUserIndex == userIndex) return;

//                     var otherUserVector = _userItemMatrix.Row(otherUserIndex);
//                     var similarity = CosineSimilarity(targetUserVector, otherUserVector);

//                     if (similarity > _settings.MinSimilarityThreshold)
//                     {
//                         var userId = _indexUserMap[otherUserIndex];
//                         lock (similarities)
//                         {
//                             similarities.Add((userId, similarity));
//                         }
//                     }
//                 });
//             });

//             return similarities
//                 .OrderByDescending(x => x.similarity)
//                 .Take(count)
//                 .ToList();
//         }

//         private List<(int itemId, double score)> GetCandidateItems(
//             List<(int userId, double similarity)> similarUsers,
//             int targetUserId)
//         {
//             var itemScores = new Dictionary<int, double>();
//             var targetUserIndex = _userIndexMap[targetUserId];
//             var targetUserVector = _userItemMatrix!.Row(targetUserIndex);

//             foreach (var (userId, similarity) in similarUsers)
//             {
//                 if (!_userIndexMap.ContainsKey(userId)) continue;

//                 var userIndex = _userIndexMap[userId];
//                 var userVector = _userItemMatrix.Row(userIndex);

//                 for (int itemIndex = 0; itemIndex < userVector.Count; itemIndex++)
//                 {
//                     var rating = userVector[itemIndex];
//                     if (rating > 0 && targetUserVector[itemIndex] == 0) // User rated item, target user hasn't
//                     {
//                         var itemId = _indexItemMap[itemIndex];

//                         if (!itemScores.ContainsKey(itemId))
//                             itemScores[itemId] = 0;

//                         itemScores[itemId] += similarity * rating;
//                     }
//                 }
//             }

//             return itemScores
//                 .Select(x => (x.Key, x.Value))
//                 .OrderByDescending(x => x.Value)
//                 .ToList();
//         }

//         private double PredictRatingFromSimilarUsers(int itemIndex, List<(int userId, double similarity)> similarUsers)
//         {
//             double weightedSum = 0;
//             double similaritySum = 0;

//             foreach (var (userId, similarity) in similarUsers)
//             {
//                 if (!_userIndexMap.ContainsKey(userId)) continue;

//                 var userIndex = _userIndexMap[userId];
//                 var rating = _userItemMatrix!.At(userIndex, itemIndex);

//                 if (rating > 0)
//                 {
//                     weightedSum += similarity * rating;
//                     similaritySum += Math.Abs(similarity);
//                 }
//             }

//             return similaritySum > 0 ? weightedSum / similaritySum : 2.5;
//         }

//         private double ConvertInteractionToRating(UserInteraction interaction)
//         {
//             // Apply time decay
//             var daysSince = (DateTime.UtcNow - interaction.Timestamp).TotalDays;
//             var timeDecay = Math.Exp(-daysSince / _settings.TimeDecaySettings.HalfLifeDays);

//             var baseRating = interaction.Type switch
//             {
//                 InteractionType.View => 1.0,
//                 InteractionType.Like => 2.5,
//                 InteractionType.AddToCart => 3.5,
//                 InteractionType.Purchase => 4.5,
//                 InteractionType.Rate => interaction.Rating,
//                 _ => 1.0
//             };

//             return baseRating * timeDecay;
//         }

//         private static double CosineSimilarity(Vector<double> a, Vector<double> b)
//         {
//             var dotProduct = a.DotProduct(b);
//             var magnitudeA = a.L2Norm();
//             var magnitudeB = b.L2Norm();

//             if (magnitudeA < 1e-10 || magnitudeB < 1e-10)
//                 return 0;

//             return dotProduct / (magnitudeA * magnitudeB);
//         }
//     }
// }