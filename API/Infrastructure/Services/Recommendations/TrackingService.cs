using Core.Entities.Recommendation;
using Core.Enums;
using Core.Interfaces.Recommendations;
using Core.Services;

namespace Infrastructure.Services.Recommendations
{
    public class TrackingService : ITrackingService
    {
        private readonly ISessionInteractionRepository _interactionRepository;
        private readonly ISessionService _sessionService;

        public TrackingService(
            ISessionInteractionRepository interactionRepository,
            ISessionService sessionService)
        {
            _interactionRepository = interactionRepository;
            _sessionService = sessionService;
        }

        public async Task TrackViewAsync(string sessionId, int productId, int? durationSeconds = null)
        {
            await TrackInteractionAsync(sessionId, productId, InteractionType.View, durationSeconds);
        }

        public async Task TrackAddToCartAsync(string sessionId, int productId, int? skuId = null)
        {
            await TrackInteractionAsync(sessionId, productId, InteractionType.AddToCart);
        }

        public async Task TrackPurchaseAsync(string sessionId, List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                await TrackInteractionAsync(sessionId, productId, InteractionType.Purchase);
            }
        }

        public async Task TrackInteractionAsync(string sessionId, int productId, InteractionType type, int? durationSeconds = null)
        {
            await _sessionService.UpdateSessionActivityAsync(sessionId);

            var interaction = new SessionInteraction
            {
                SessionId = sessionId,
                ProductId = productId,
                InteractionType = type,
                InteractionDate = DateTime.UtcNow,
                DurationSeconds = durationSeconds
            };

            await _interactionRepository.AddAsync(interaction);
        }
    }
}