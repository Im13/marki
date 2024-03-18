using Core.Entities.ShopeeOrder;

namespace Core.Specification
{
    public class ShopeeOrderSpecification : BaseSpecification<ShopeeOrder>
    {
        public ShopeeOrderSpecification(ShopeeOrderSpecParams shopeeOrderParams) : base(x => 
            (string.IsNullOrEmpty(shopeeOrderParams.Search) || x.OrderId.Contains(shopeeOrderParams.Search)) &&
            (string.IsNullOrEmpty(shopeeOrderParams.Date) || x.OrderDate.Date == DateTime.ParseExact(shopeeOrderParams.Date, "dd/MM/yyyy", null).Date))
        {
            AddInclude(x => x.Products);
            AddOrderByDescending(x => x.OrderDate);
            ApplyPaging(shopeeOrderParams.PageSize * (shopeeOrderParams.PageIndex - 1), shopeeOrderParams.PageSize);
        }
    }
}