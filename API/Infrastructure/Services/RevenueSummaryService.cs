using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;

public class RevenueSummaryService : IRevenueSummaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<RevenueSummary> _revenueSummaryRepository;


    public RevenueSummaryService(
        IGenericRepository<RevenueSummary> revenueSummaryRepository,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _revenueSummaryRepository = revenueSummaryRepository;
    }

    public async Task UpdateRevenueSummaryFromOrder(Order order)
    {
        var date = order.OrderDate.Date;
        var spec = new RevenueSummarySpecification(date);
        var summary = await _revenueSummaryRepository.GetEntityWithSpec(spec);

        // if (summary == null)
        // {
        //     summary = new RevenueSummary
        //     {
        //         Date = date,
        //         PaymentSource = order.PaymentSource,
        //         DailyRevenue = order.TotalAmount,
        //         OrderCount = 1
        //     };
        //     _revenueSummaryRepository.Add(summary);
        // }
        // else
        // {
        //     summary.DailyRevenue += order.TotalAmount;
        //     summary.OrderCount++;
        //     _revenueSummaryRepository.Update(summary);
        // }

        await _unitOfWork.Complete();
    }

    // public async Task<Dictionary<DateTime, decimal>> GetDailyRevenue(DateTime startDate, DateTime endDate)
    // {
    //     var summaries = await _revenueSummaryRepository.GetQuery()
    //         .Where(x => x.Date >= startDate && x.Date <= endDate)
    //         .GroupBy(x => x.Date)
    //         .Select(g => new
    //         {
    //             Date = g.Key,
    //             Revenue = g.Sum(x => x.DailyRevenue)
    //         })
    //         .ToListAsync();

    //     return summaries.ToDictionary(x => x.Date, x => x.Revenue);
    // }

    // public Task<Dictionary<string, decimal>> GetRevenueBySource(DateTime startDate, DateTime endDate)
    // {
    //     throw new NotImplementedException();
    // }

    // Implement các method khác
}