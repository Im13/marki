using Core.Entities.ShopeeOrder;

namespace Core.Specification 
{
    public class ShopeeOrderWithFilterForCountSpecification : BaseSpecification<ShopeeOrder>
    {
        public ShopeeOrderWithFilterForCountSpecification(ShopeeOrderSpecParams shopeeOrderSpecParams) : base(x => 
            string.IsNullOrEmpty(shopeeOrderSpecParams.Search) || x.OrderId.ToLower().Contains(shopeeOrderSpecParams.Search))
        {
            
        }
    }
}