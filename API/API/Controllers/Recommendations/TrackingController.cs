using Core.DTOs.Recommendations;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Recommendations
{
    public class TrackingController : BaseApiController
    {
        private readonly ITrackingService _trackingService;
        private readonly ISessionService _sessionService;

        public TrackingController(
            ITrackingService trackingService,
            ISessionService sessionService)
        {
            _trackingService = trackingService;
            _sessionService = sessionService;
        }

        [HttpPost("view")]
        public async Task<IActionResult> TrackView([FromBody] TrackInteractionRequest request)
        {
            var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);
            await _trackingService.TrackViewAsync(sessionId, request.ProductId, request.DurationSeconds);
            return Ok();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> TrackCart([FromBody] TrackInteractionRequest request)
        {
            var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);
            await _trackingService.TrackAddToCartAsync(sessionId, request.ProductId, request.SkuId);
            return Ok();
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> TrackPurchase([FromBody] List<int> productIds)
        {
            var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);
            await _trackingService.TrackPurchaseAsync(sessionId, productIds);
            return Ok();
        }
    }
}