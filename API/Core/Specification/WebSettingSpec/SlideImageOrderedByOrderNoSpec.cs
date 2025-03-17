using Core.Entities;

namespace Core.Specification.WebSettingSpec
{
    public class SlideImageOrderedByOrderNoSpec : BaseSpecification<SlideImage>
    {
        public SlideImageOrderedByOrderNoSpec()
        : base()
        {
            AddOrderBy(s => s.OrderNo);
        }

    }
}