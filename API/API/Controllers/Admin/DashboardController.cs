using API.DTOs.Revenue;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    public class DashboardController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IRevenueSummaryService _revenueSummaryService;
        public DashboardController(IRevenueSummaryService revenueSummaryService, IMapper mapper)
        {
            _revenueSummaryService = revenueSummaryService;
            _mapper = mapper;
        }

        [HttpGet("daily/{date}")]
        public async Task<ActionResult<RevenueSummaryDto>> GetDailyRevenue(DateTime date)
        {
            var revenue = await _revenueSummaryService.GetDailyRevenueAsync(date);
            var revenueDto = _mapper.Map<RevenueSummary, RevenueSummaryDto>(revenue);
            return Ok(revenueDto);
        }


        [HttpGet("range")]
        public async Task<ActionResult<RevenueSummaryDto>> GetRevenueInRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenue = await _revenueSummaryService.GetRevenueInRangeAsync(startDate, endDate);
            var revenueDto = _mapper.Map<RevenueSummary, RevenueSummaryDto>(revenue);
            return Ok(revenueDto);
        }

        [HttpGet("last-14-days")]
        public async Task<ActionResult<List<RevenueSummaryDto>>> GetLast14DaysRevenue()
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-14);
            var revenueData = await _revenueSummaryService.GetDashboardRevenueDataAsync(startDate, endDate);
            var revenueDto = _mapper.Map<IReadOnlyList<RevenueSummary>, IReadOnlyList<RevenueSummaryDto>>(revenueData);
            
            return Ok(revenueDto);
        }
    }
}