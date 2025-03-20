using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SlideImageRepository : GenericRepository<SlideImage>, ISlideImageRepository
    {
        private readonly StoreContext _context;

        public SlideImageRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetNextOrderNoAsync()
        {
            var maxOrderNo = await _context.SlideImages.MaxAsync(s => (int?)s.OrderNo) ?? 0;
            return maxOrderNo + 1;
        }
    }
}