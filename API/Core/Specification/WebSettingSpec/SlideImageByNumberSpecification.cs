using Core.Entities;

namespace Core.Specification.WebSettingSpec
{
    public class SlideImageByNumberSpecification : BaseSpecification<SlideImage>
    {
        public SlideImageByNumberSpecification(int orderNo)
        : base(s => s.OrderNo == orderNo)
        {
        }
    }
}