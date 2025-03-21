using Core.Entities;

namespace Core.Specification.WebSettingSpec
{
    public class SlideImageSpecification : BaseSpecification<SlideImage>
    {
        // This constructor is used to get the all slide image order by OrderNo
        public SlideImageSpecification()
        : base()
        {
            AddOrderBy(s => s.OrderNo);
        }

        // This constructor is used to get the slide image by order number
        public SlideImageSpecification(int orderNo)
        : base(s => s.OrderNo == orderNo)
        {
        }

        public static SlideImageSpecification WithActiveStatus()
        {
            return new SlideImageSpecification(true);
        }

        private SlideImageSpecification(bool onlyActive)
            : base(s => s.Status == true) 
        {
            AddOrderBy(s => s.OrderNo);
        }
    }
}