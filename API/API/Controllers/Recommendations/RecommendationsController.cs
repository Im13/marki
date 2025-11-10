using API.DTOs.Recommendation;
using Core.DTOs.Recommendations;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Recommendations
{
    public class RecommendationsController : BaseApiController
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<RecommendationsController> _logger;
        private readonly ITrackingService _trackingService;
        
        public RecommendationsController(
            IRecommendationService recommendationService,
            ISessionService sessionService,
            ILogger<RecommendationsController> logger,
            ITrackingService trackingService)
        {
            _recommendationService = recommendationService;
            _sessionService = sessionService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RecommendationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RecommendationDTO>>> GetRecommendations(
            [FromQuery] int limit = 10)
        {
            try
            {
                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

                _logger.LogInformation(
                    "Getting {Limit} recommendations for session {SessionId}",
                    limit, sessionId);

                var recommendations = await _recommendationService
                    .GetRecommendationsAsync(sessionId, limit);

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommendations");
                return StatusCode(500, new { message = "Error loading recommendations" });
            }
        }

        [HttpGet("trending")]
        [ProducesResponseType(typeof(List<RecommendationDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RecommendationDTO>>> GetTrending(
            [FromQuery] int limit = 10)
        {
            try
            {
                var trending = await _recommendationService.GetTrendingProductsAsync(limit);

                return Ok(trending);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trending products");
                return StatusCode(500, new { message = "Error loading trending products" });
            }
        }

        [HttpGet("similar/{productId}")]
        [ProducesResponseType(typeof(List<RecommendationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<RecommendationDTO>>> GetSimilar(
            int productId,
            [FromQuery] int limit = 8)
        {
            try
            {
                var similar = await _recommendationService.GetSimilarProductsAsync(productId, limit);

                if (similar == null || !similar.Any())
                {
                    return Ok(new List<RecommendationDTO>());
                }

                return Ok(similar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error getting similar products for product {ProductId}", productId);
                return StatusCode(500, new { message = "Error loading similar products" });
            }
        }

        [HttpPost("track/view")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TrackView([FromBody] TrackViewRequest request)
        {
            try
            {
                if (request == null || request.ProductId <= 0)
                {
                    return BadRequest(new { message = "Invalid request" });
                }

                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

                await _trackingService.TrackViewAsync(
                    sessionId,
                    request.ProductId,
                    request.DurationSeconds);

                _logger.LogDebug(
                    "Tracked view: Session={SessionId}, Product={ProductId}, Duration={Duration}s",
                    sessionId, request.ProductId, request.DurationSeconds);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking view for product {ProductId}",
                    request?.ProductId);
                // Return OK anyway to not block user experience
                return Ok(new { success = false });
            }
        }

        [HttpPost("track/cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TrackCart([FromBody] TrackCartRequest request)
        {
            try
            {
                if (request == null || request.ProductId <= 0)
                {
                    return BadRequest(new { message = "Invalid request" });
                }

                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

                await _trackingService.TrackAddToCartAsync(
                    sessionId,
                    request.ProductId,
                    request.SkuId);

                _logger.LogDebug(
                    "Tracked add to cart: Session={SessionId}, Product={ProductId}, SKU={SkuId}",
                    sessionId, request.ProductId, request.SkuId);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking cart for product {ProductId}",
                    request?.ProductId);
                return Ok(new { success = false });
            }
        }

        [HttpPost("track/purchase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TrackPurchase([FromBody] TrackPurchaseRequest request)
        {
            try
            {
                if (request == null || request.ProductIds == null || !request.ProductIds.Any())
                {
                    return BadRequest(new { message = "Invalid request" });
                }

                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

                await _trackingService.TrackPurchaseAsync(sessionId, request.ProductIds);

                _logger.LogInformation(
                    "Tracked purchase: Session={SessionId}, Products={ProductCount}",
                    sessionId, request.ProductIds.Count);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking purchase");
                return Ok(new { success = false });
            }
        }

        [HttpPost("track/click")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TrackClick([FromBody] TrackClickRequest request)
        {
            try
            {
                if (request == null || request.ProductId <= 0)
                {
                    return BadRequest(new { message = "Invalid request" });
                }

                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

                _logger.LogInformation(
                    "Recommendation clicked: Session={SessionId}, Product={ProductId}, Reason={ReasonCode}",
                    sessionId, request.ProductId, request.ReasonCode);

                // Optional: Store in database for metrics
                // await _trackingService.TrackRecommendationClickAsync(sessionId, request.ProductId, request.ReasonCode);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking click");
                return Ok(new { success = false });
            }
        }

        /// <summary>
        /// Track wishlist add
        /// </summary>
        // [HttpPost("track/wishlist")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public async Task<IActionResult> TrackWishlist([FromBody] TrackWishlistRequest request)
        // {
        //     try
        //     {
        //         if (request == null || request.ProductId <= 0)
        //         {
        //             return BadRequest(new { message = "Invalid request" });
        //         }

        //         var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);

        //         await _trackingService.TrackInteractionAsync(
        //             sessionId,
        //             request.ProductId,
        //             InteractionType.Wishlist);

        //         _logger.LogDebug(
        //             "Tracked wishlist: Session={SessionId}, Product={ProductId}",
        //             sessionId, request.ProductId);

        //         return Ok(new { success = true });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error tracking wishlist");
        //         return Ok(new { success = false });
        //     }
        // }

        [HttpGet("session")]
        [ProducesResponseType(typeof(SessionInfoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SessionInfoDto>> GetSessionInfo()
        {
            try
            {
                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);
                var session = await _sessionService.GetSessionAsync(sessionId);

                if (session == null)
                {
                    return NotFound(new { message = "Session not found" });
                }

                var info = new SessionInfoDto
                {
                    SessionId = session.SessionId,
                    CreatedAt = session.CreatedAt,
                    LastActivityAt = session.LastActivityAt,
                    InteractionCount = session.Interactions?.Count ?? 0
                };

                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting session info");
                return StatusCode(500, new { message = "Error getting session info" });
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "RecommendationService"
            });
        }
    }
}