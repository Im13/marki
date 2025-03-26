using API.DTOs.Revenue;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    public class DashboardController : BaseApiController
    {
        private readonly IRevenueSummaryService _revenueSummaryService;
        public DashboardController(IRevenueSummaryService revenueSummaryService)
        {
            _revenueSummaryService = revenueSummaryService;
        }

        [HttpGet("daily/{date}")]
        public async Task<ActionResult<RevenueSummaryDto>> GetDailyRevenue(DateTime date)
        {
            var revenue = await _revenueSummaryService.GetDailyRevenueAsync(date);
            return Ok(revenue);
        }


        [HttpGet("range")]
        public async Task<ActionResult<DashboardRevenueResponseDTO>> GetRevenueInRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _revenueSummaryService.GetDashboardRevenueDataAsync(startDate, endDate);
            return Ok(result);
        }
    }
}