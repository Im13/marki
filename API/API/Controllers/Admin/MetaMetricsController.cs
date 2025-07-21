using API.DTOs;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    public class MetaMetricsController : BaseApiController
    {
        private readonly IFacebookMarketingService _facebookMarketingService;
        private readonly IMapper _mapper;

        public MetaMetricsController(IFacebookMarketingService facebookMarketingService, IMapper mapper)
        {
            _mapper = mapper;
            _facebookMarketingService = facebookMarketingService;
        }

        [HttpGet("campaigns")]
        public async Task<ActionResult<IReadOnlyList<CampaignWithAdsetsDTO>>> GetCampaignsWithAdsets([FromQuery] string since, [FromQuery] string until)
        {
            if (!DateTime.TryParse(since, out var sinceDate) ||
                !DateTime.TryParse(until, out var untilDate))
            {
                return BadRequest("Invalid 'since' or 'until' date. Use format yyyy-MM-dd.");
            }

            try
            {
                var data = await _facebookMarketingService.GetActiveCampaignsWithAdsetsInsightsAsync(sinceDate, untilDate);
                return _mapper.Map<List<CampaignWithAdsetsDTO>>(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}