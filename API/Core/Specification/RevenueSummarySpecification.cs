using Core.Entities;
using Core.Specification;

namespace Core.Specification
{
    public class RevenueSummarySpecification : BaseSpecification<RevenueSummary>
    {
        public RevenueSummarySpecification(DateTime date)
            : base(x => x.Date.Date == date.Date)
        {
        }

        // Thêm specification cho lấy doanh thu theo khoảng thời gian
        public RevenueSummarySpecification(DateTime startDate, DateTime endDate)
            : base(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
        {
            AddOrderByDescending(x => x.Date);
        }

        // Thêm specification cho lấy doanh thu theo nguồn thanh toán
        // public RevenueSummarySpecification(string paymentSource)
        //     : base(x => x.PaymentSource == paymentSource)
        // {
        //     AddOrderByDescending(x => x.Date);
        // }
    }
}
