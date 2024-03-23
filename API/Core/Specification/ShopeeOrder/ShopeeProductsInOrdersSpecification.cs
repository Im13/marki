using Core.Entities.ShopeeOrder;

namespace Core.Specification
{
    public class ShopeeProductsInOrdersSpecification : BaseSpecification<ShopeeOrder>
    {
        public ShopeeProductsInOrdersSpecification(ShopeeOrderSpecParams shopeeOrderParams) : base(x => 
            (string.IsNullOrEmpty(shopeeOrderParams.Search) || x.OrderId.Contains(shopeeOrderParams.Search)) &&
            (string.IsNullOrEmpty(shopeeOrderParams.Date) || x.OrderDate.Date == DateTime.ParseExact(shopeeOrderParams.Date, "dd/MM/yyyy", null).Date))
        {
            AddInclude(x => x.Products);
            AddOrderByDescending(x => x.OrderDate);
        }
    }
}