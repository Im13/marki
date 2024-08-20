using Core;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private StoreContext _context;
        public OrderService(
            IUnitOfWork unitOfWork,
            IBasketRepository basketRepo,
            StoreContext context)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // Get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // Get items from the product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                //Need fix with sku price
                var itemOrdered = new ProductIemOrdered(productItem.Id, productItem.Name, productItem.ProductSKUs.First().ImageUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.ProductSKUs.First().Price, item.Quantity);
                items.Add(orderItem);
            }

            // Get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // Calc subtotal
            var subTotal = items.Sum(item => item.Price * item.Quantity);

            // Create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal);

            _unitOfWork.Repository<Order>().Add(order);

            // Save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // Delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            // Return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<OfflineOrder> CreateOfflineOrder(OfflineOrder order, int provinceId, int wardId, int districtId)
        {
            if (order.Id != 0) return null;

            if (order.OfflineOrderSKUs.Count > 0)
            {
                foreach (var skuItems in order.OfflineOrderSKUs)
                {
                    //Remove this when edit nullable productskuid
                    int productSkuId = (int)skuItems.ProductSkuId;
                    skuItems.ProductSKU = await _unitOfWork.Repository<ProductSKUs>().GetByIdAsync(productSkuId);
                }
            }

            var province = await _unitOfWork.Repository<Province>().GetByIdAsync(provinceId);
            var ward = await _unitOfWork.Repository<Ward>().GetByIdAsync(wardId);
            var district = await _unitOfWork.Repository<District>().GetByIdAsync(districtId);

            if(province == null || ward == null || district == null) return null;

            order.Province = province;
            order.Ward = ward;
            order.District = district;
            
            //Define orderStatus 
            var status = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(0);
            order.OrderStatus = status;

            _unitOfWork.Repository<OfflineOrder>().Add(order);

            var saveOrderResult = await _unitOfWork.Complete();

            if (saveOrderResult <= 0) return null;

            return order;
        }

        public async Task<OfflineOrder> UpdateOfflineOrder(OfflineOrder order, List<OfflineOrderSKUs> currentListSkus)
        {
            if (order.Id < 0) return null;

            var currentOrder = await _context.OfflineOrders.Include(o => o.OfflineOrderSKUs).Where(o => o.Id == order.Id).SingleOrDefaultAsync();
            _unitOfWork.ClearTracker();

            //Delete all childs, need improment later
            foreach(var sku in currentOrder.OfflineOrderSKUs)
            {
                _context.OfflineOrderSKUs.Remove(sku);
            }

            if (order.OfflineOrderSKUs.Count > 0)
            {
                foreach (var skuItems in order.OfflineOrderSKUs)
                {
                    //Remove this when edit nullable productskuid
                    int productSkuId = (int)skuItems.ProductSkuId;

                    // var checkOrder = currentListSkus.SingleOrDefault()
                    skuItems.ProductSKU = await _unitOfWork.Repository<ProductSKUs>().GetByIdAsync(productSkuId);
                }
            }

            _unitOfWork.Repository<OfflineOrder>().Update(order);

            var saveOrderResult = await _unitOfWork.Complete();

            if (saveOrderResult <= 0) return null;

            return order;
        }

        public async Task<OfflineOrder> GetOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<OfflineOrder>().GetByIdAsync(id);
            _unitOfWork.ClearTracker();

            return order;
        }
    }
}