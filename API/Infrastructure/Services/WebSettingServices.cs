using Core.Entities;
using Core.Interfaces;
using Core.Specification.WebSettingSpec;

namespace Infrastructure.Services
{
    public class WebSettingServices : IWebSettingServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebSettingServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SlideImage> CreateSlide(SlideImage slideImage) 
        {
            if(slideImage.Id != 0) return null;

            var newOrderNo = await _unitOfWork.SlideImageRepository.GetNextOrderNoAsync();

            slideImage.OrderNo = newOrderNo;

            _unitOfWork.Repository<SlideImage>().Add(slideImage);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return slideImage;
        }


        public async Task<SlideImage> UpdateSlide(SlideImage slideImage)
        {
            var slide = await _unitOfWork.Repository<SlideImage>().GetByIdAsync(slideImage.Id);

            if(slide == null) return null;

            var slideByOrderNo = await _unitOfWork.Repository<SlideImage>().GetFirstOrDefaultAsync(s => s.OrderNo == slideImage.OrderNo);

            if(slideByOrderNo != null && slideByOrderNo.Id != slideImage.Id) {
                (slideByOrderNo.OrderNo, slideImage.OrderNo) = (slideImage.OrderNo, slideByOrderNo.OrderNo);
                
                _unitOfWork.Repository<SlideImage>().Update(slideByOrderNo);
            }

            slide.DesktopImageUrl = slideImage.DesktopImageUrl;
            slide.AltText = slideImage.AltText;
            slide.Link = slideImage.Link;
            slide.MobileImageUrl = slideImage.MobileImageUrl;
            slide.OrderNo = slideImage.OrderNo;
            slide.Status = slideImage.Status;

            _unitOfWork.Repository<SlideImage>().Update(slide);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return slide;
        }

        public async Task<IReadOnlyList<SlideImage>> GetSlides()
        {
            return await _unitOfWork.Repository<SlideImage>().ListAsync(new SlideImageOrderedByOrderNoSpec());
        }
    }
}