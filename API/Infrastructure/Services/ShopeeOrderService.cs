using Core.Entities.ShopeeOrder;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ShopeeOrderService : IShopeeOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopeeOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateOrdersAsync(List<ShopeeOrder> orders)
        {
            if(orders.Count == 0)
            {
                return false;
            }

            var allOrders = await _unitOfWork.Repository<ShopeeOrder>().ListAllAsync();

            foreach(var order in orders) {
                if(allOrders.FirstOrDefault(o => o.OrderId == order.OrderId) == null) 
                {
                    _unitOfWork.Repository<ShopeeOrder>().Add(order);
                }
            }

            return await _unitOfWork.Complete() > 0;
        }
    }
}