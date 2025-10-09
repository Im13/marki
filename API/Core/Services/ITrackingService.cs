namespace Core.Services
{
    public interface ITrackingService
    {
        Task TrackViewAsync(string sessionId, int productId, int? durationSeconds = null);
        Task TrackAddToCartAsync(string sessionId, int productId, int? skuId = null);
        Task TrackPurchaseAsync(string sessionId, List<int> productIds);
    }
}