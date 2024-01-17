using Core.Entities.ShopeeOrder;

namespace Core.Interfaces
{
    public interface IShopeeOrderService 
    {
        Task<bool> CreateOrdersAsync(List<ShopeeOrder> orders);
    }
}