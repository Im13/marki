using Core.Entities;
using Core.Specification.PhotoSpec;

namespace Core.Specification
{
    public class PhotosWithProductFilterSpecification : BaseSpecification<Photo>
    {
        public PhotosWithProductFilterSpecification(PhotoSpecParams photoParams) : base(x => 
            (!photoParams.ProductId.HasValue || x.Product.Id == photoParams.ProductId)
        )
        {
        }

        public PhotosWithProductFilterSpecification(int productId) : base(p => p.ProductId == productId)
        {}
    }
}