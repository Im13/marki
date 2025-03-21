using Core.Entities;

namespace Core.Interfaces
{
    public interface IWebSettingServices
    {
        Task<SlideImage> CreateSlide(SlideImage slideImage);
        Task<SlideImage> UpdateSlide(SlideImage slideImage);
        Task<IReadOnlyList<SlideImage>> GetSlides();
        Task<IReadOnlyList<SlideImage>> GetActiveSlides();
    }
}