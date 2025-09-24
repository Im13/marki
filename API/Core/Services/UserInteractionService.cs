// using API.Core.DTOs;
// using Core.Entities.RecommendedData;
// using Core.Enums;
// using Core.Interfaces;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.Logging;

// namespace Core.Services
// {
//     public class UserInteractionService : IUserInteractionService
//     {
//         private readonly IUserInteractionRepository _interactionRepository;
//         private readonly IUserSyncService _userSyncService;
//         private readonly IMemoryCache _cache;
//         private readonly ILogger<UserInteractionService> _logger;

//         public UserInteractionService(
//             IUserInteractionRepository interactionRepository,
//             IUserSyncService userSyncService,
//             IMemoryCache cache,
//         ILogger<UserInteractionService> logger)
//         {
//             _interactionRepository = interactionRepository;
//             _userSyncService = userSyncService;
//             _cache = cache;
//             _logger = logger;
//         }

//         public async Task TrackInteractionAsync(int identityUserId, int productId, InteractionType type, double? rating = null, int? duration = null)
//         {
//             try
//             {
//                 var appUser = await _userSyncService.GetAppUserByIdentityIdAsync(identityUserId);
//                 if (appUser == null)
//                 {
//                     await _userSyncService.CreateAppUserAsync(identityUserId, "User");
//                     appUser = await _userSyncService.GetAppUserByIdentityIdAsync(identityUserId);
//                 }

//                 if (appUser == null)
//                 {
//                     _logger.LogError("Failed to create or find app user for identity user {IdentityUserId}", identityUserId);
//                     return;
//                 }

//                 // Check for duplicate interactions (within last minute)
//                 var recentInteraction = await _interactionRepository.GetLatestInteractionAsync(appUser.Id, productId);
//                 if (recentInteraction != null &&
//                     recentInteraction.Type == type &&
//                     (DateTime.UtcNow - recentInteraction.Timestamp).TotalMinutes < 1)
//                 {
//                     return; // Skip duplicate interaction
//                 }

//                 var interaction = new UserInteraction
//                 {
//                     AppUserId = appUser.Id,
//                     ProductId = productId,
//                     Type = type,
//                     Rating = rating ?? 0,
//                     Duration = duration ?? 0,
//                     Timestamp = DateTime.UtcNow
//                 };

//                 await _interactionRepository.AddAsync(interaction);
//                 await _userSyncService.UpdateAppUserLastActiveAsync(appUser.Id);

//                 // Clear user's cached stats
//                 _cache.Remove($"user_stats_{identityUserId}");
//                 _cache.Remove($"user_interactions_{identityUserId}");

//                 _logger.LogDebug("Tracked {InteractionType} interaction for user {UserId} on product {ProductId}",
//                     type, identityUserId, productId);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error tracking interaction for user {UserId} on product {ProductId}",
//                     identityUserId, productId);
//             }
//         }

//         public async Task<List<UserInteraction>> GetUserInteractionsAsync(int identityUserId, int? limit = null)
//         {
//             var cacheKey = $"user_interactions_{identityUserId}_{limit}";

//             if (_cache.TryGetValue(cacheKey, out List<UserInteraction>? cachedInteractions) && cachedInteractions != null)
//             {
//                 return cachedInteractions;
//             }

//             var appUser = await _userSyncService.GetAppUserByIdentityIdAsync(identityUserId);
//             if (appUser == null) return new List<UserInteraction>();

//             var interactions = await _interactionRepository.GetUserInteractionsAsync(appUser.Id, limit);
//             _cache.Set(cacheKey, interactions, TimeSpan.FromMinutes(10));

//             return interactions;
//         }

//         public async Task<UserInteractionStats> GetUserStatsAsync(int identityUserId)
//         {
//             var cacheKey = $"user_stats_{identityUserId}";

//             if (_cache.TryGetValue(cacheKey, out UserInteractionStats? cachedStats) && cachedStats != null)
//             {
//                 return cachedStats;
//             }

//             var appUser = await _userSyncService.GetAppUserByIdentityIdAsync(identityUserId);
//             if (appUser == null) return new UserInteractionStats();

//             var interactions = await _interactionRepository.GetUserInteractionsAsync(appUser.Id);

//             var stats = new UserInteractionStats
//             {
//                 TotalInteractions = interactions.Count,
//                 TotalViews = interactions.Count(i => i.Type == InteractionType.View),
//                 TotalLikes = interactions.Count(i => i.Type == InteractionType.Like),
//                 TotalCartAdds = interactions.Count(i => i.Type == InteractionType.AddToCart),
//                 TotalPurchases = interactions.Count(i => i.Type == InteractionType.Purchase),
//                 TotalRatings = interactions.Count(i => i.Type == InteractionType.Rate),
//                 AverageRating = interactions.Where(i => i.Type == InteractionType.Rate && i.Rating > 0)
//                                          .DefaultIfEmpty()
//                                          .Average(i => i?.Rating ?? 0),
//                 LastInteractionAt = interactions.OrderByDescending(i => i.Timestamp)
//                                               .FirstOrDefault()?.Timestamp,
//                 CategoryInteractions = interactions
//                     .Where(i => i.Product != null)
//                     .GroupBy(i => i.Product.Category.Name)
//                     .ToDictionary(g => g.Key, g => g.Count()),
//                 InteractionsByType = interactions
//                     .GroupBy(i => i.Type)
//                     .ToDictionary(g => g.Key, g => g.Count())
//             };

//             _cache.Set(cacheKey, stats, TimeSpan.FromMinutes(15));
//             return stats;
//         }
//     }
// }