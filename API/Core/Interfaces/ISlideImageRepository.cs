using Core.Entities;

namespace Core.Interfaces
{
    public interface ISlideImageRepository : IGenericRepository<SlideImage>
    {
        Task<int> GetNextOrderNoAsync();
    }
}