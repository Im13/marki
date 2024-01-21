using Core.Entities.ShopeeOrder;

namespace Core.Interfaces
{
    public interface IShopeeOrderService 
    {
        Task<List<ShopeeOrder>> CreateOrdersAsync(List<ShopeeOrder> orders);
    }
}