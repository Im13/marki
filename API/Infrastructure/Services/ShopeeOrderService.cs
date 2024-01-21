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

        public async Task<List<ShopeeOrder>> CreateOrdersAsync(List<ShopeeOrder> orders)
        {
            List<ShopeeOrder> addedList = new List<ShopeeOrder>();

            if(orders.Count == 0)
                return addedList;

            var allOrders = await _unitOfWork.Repository<ShopeeOrder>().ListAllAsync();

            foreach(var order in orders) {
                if(allOrders.FirstOrDefault(o => o.OrderId == order.OrderId) == null) 
                {
                    _unitOfWork.Repository<ShopeeOrder>().Add(order);
                    addedList.Add(order);
                }
            }

            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            return addedList;
        }
    }
}