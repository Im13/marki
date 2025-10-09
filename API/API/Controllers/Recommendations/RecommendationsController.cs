using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public RecommendationsController(
            IRecommendationService recommendationService,
            ISessionService sessionService,
            ILogger<RecommendationsController> logger)
        {
            _recommendationService = recommendationService;
            _sessionService = sessionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecommendationDTO>>> GetRecommendations(
            [FromQuery] int limit = 10)
        {
            try
            {
                var sessionId = await _sessionService.GetOrCreateSessionIdAsync(HttpContext);
                var recommendations = await _recommendationService.GetRecommendationsAsync(sessionId, limit);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommendations");
                return StatusCode(500, "Error loading recommendations");
            }
        }

        [HttpGet("trending")]
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
                return StatusCode(500, "Error loading trending products");
            }
        }

        [HttpGet("similar/{productId}")]
        public async Task<ActionResult<List<RecommendationDTO>>> GetSimilar(
            int productId, 
            [FromQuery] int limit = 8)
        {
            try
            {
                var similar = await _recommendationService.GetSimilarProductsAsync(productId, limit);
                return Ok(similar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting similar products for product {ProductId}", productId);
                return StatusCode(500, "Error loading similar products");
            }
        }
    }
}