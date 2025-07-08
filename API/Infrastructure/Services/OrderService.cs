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
        private readonly IProductRepository _productRepo;
        private readonly IRevenueSummaryRepository _revenueRepo;
        private readonly StoreContext _context;
        public OrderService(
            IUnitOfWork unitOfWork,
            IBasketRepository basketRepo,
            IProductRepository productRepo,
            IRevenueSummaryRepository revenueRepo,
            StoreContext context)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _context = context;
            _productRepo = productRepo;
            _revenueRepo = revenueRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress, decimal shippingFee, decimal orderDiscount, decimal bankTransferedAmount, decimal extraFee, decimal total, string orderNote)
        {
            // Get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // Get items from the product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _productRepo.GetProductByIdAsync(item.ProductId);

                //Need fix with sku price
                var itemOrdered = new ProductIemOrdered(productItem.Id, productItem.Name, item.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.ProductSKUs.First().Price, item.Quantity, item.OptionValueCombination);
                items.Add(orderItem);
            }

            // Get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // Calc subtotal
            var subTotal = items.Sum(item => item.Price * item.Quantity);

            //Define orderStatus 
            var status = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(1);

            //CreateCustomer
            var customer = new Customer {
                Name = shippingAddress.Fullname,
                PhoneNumber = shippingAddress.PhoneNumber,
                EmailAddress = buyerEmail,
                IsDeleted = false
            };

            // Create order
            var order = new Order(items, buyerEmail, deliveryMethod, subTotal, OrderSources.Website, shippingFee, orderDiscount, bankTransferedAmount, extraFee, total, orderNote, status);
            order.Fullname = shippingAddress.Fullname;
            order.CityOrProvinceId = shippingAddress.CityOrProvinceId;
            order.DistrictId = shippingAddress.DistrictId;
            order.WardId = shippingAddress.WardId;
            order.Street = shippingAddress.Street;
            order.PhoneNumber = shippingAddress.PhoneNumber;
            order.Customer = customer;

            _unitOfWork.Repository<Order>().Add(order);

            // Save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // Cập nhật doanh thu
            await _revenueRepo.UpdateRevenueAsync(order);

            // Delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            // Return order
            return order;
        }

        public async Task<Order> CreateOrderFromAdminAsync(Order order)
        {
            if (order.Id != 0) return null;

            _unitOfWork.Repository<Order>().Add(order);

            // Get delivery, but we will remove this because delivery method is used only to check paid order.
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(0);

            order.DeliveryMethod = deliveryMethod;
            order.OrderStatus = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(1);
            order.OrderDate = DateTime.UtcNow;
            order.Source = OrderSources.Offline;
            order.Customer = new Customer
            {
                Name = order.Fullname,
                PhoneNumber = order.PhoneNumber,
                EmailAddress = order.BuyerEmail,
                IsDeleted = false
            };
            
            // Save to db
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            // Cập nhật doanh thu
            await _revenueRepo.UpdateRevenueAsync(order);

            return null;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id)
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(id);
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

            if (province == null || ward == null || district == null) return null;

            order.Province = province;
            order.Ward = ward;
            order.District = district;
            order.DateCreated = order.DateCreated.ToLocalTime();

            //Define orderStatus 
            var status = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(1);
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

            var orderStatus = await _context.OrderStatus.Where(s => s.Id == order.OrderStatus.Id).SingleOrDefaultAsync();
            order.OrderStatus = orderStatus;

            //Delete all childs, need improment later
            foreach (var sku in currentOrder.OfflineOrderSKUs)
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

        public async Task<Order> UpdateOrder(Order order, List<OrderItem> items)
        {
            if (order.Id < 0) return null;

            var currentOrder = await _context.Orders.Include(o => o.OrderItems).Where(o => o.Id == order.Id).SingleOrDefaultAsync();
            if (currentOrder == null) return null;

            //Get Customer
            var customer = await _context.Customers.Where(c => c.Id == order.Customer.Id).SingleOrDefaultAsync();

            if(customer == null) return null;

            customer.Name = order.Customer.Name;
            customer.PhoneNumber = order.Customer.PhoneNumber;
            customer.EmailAddress = order.Customer.EmailAddress;
            customer.DOB = order.Customer.DOB;

            // Update other order properties if needed
            currentOrder.BuyerEmail = order.BuyerEmail;
            currentOrder.DeliveryMethod = order.DeliveryMethod;
            currentOrder.Subtotal = order.Subtotal;
            currentOrder.ShippingFee = order.ShippingFee;
            currentOrder.OrderDiscount = order.OrderDiscount;
            currentOrder.BankTransferedAmount = order.BankTransferedAmount;
            currentOrder.ExtraFee = order.ExtraFee;
            currentOrder.Total = order.Total;
            currentOrder.OrderNote = order.OrderNote;
            currentOrder.Fullname = order.Fullname;
            currentOrder.CityOrProvinceId = order.CityOrProvinceId;
            currentOrder.DistrictId = order.DistrictId;
            currentOrder.WardId = order.WardId;
            currentOrder.Street = order.Street;
            currentOrder.PhoneNumber = order.PhoneNumber;
            currentOrder.Customer = customer;

            // Danh sách OrderItems mới từ client
            var updatedItems = order.OrderItems;

            currentOrder.OrderItems.RemoveAll(oi => !updatedItems.Any(ui => ui.Id == oi.Id));

            foreach (var item in items)
            {
                var existingItem = currentOrder.OrderItems.FirstOrDefault(oi => oi.Id == item.Id);

                if (existingItem != null)
                {
                    existingItem.ProductName = item.ProductName;
                    existingItem.Price = item.Price;
                    existingItem.Quantity = item.Quantity;
                    existingItem.OptionValueCombination = item.OptionValueCombination;
                }
                else
                {
                    currentOrder.OrderItems.Add(new OrderItem
                    {
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        OptionValueCombination = item.OptionValueCombination,
                        ItemOrdered = item.ItemOrdered
                    });
                }
            }

            _unitOfWork.Repository<Order>().Update(currentOrder);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new Exception("Failed to update order.");
            }

            await _revenueRepo.UpdateDailyRevenueAsync(currentOrder);

            var revenueResult = await _unitOfWork.Complete();

            if (revenueResult <= 0)
            {
                throw new Exception("Failed to update revenue.");
            }

            return order;
        }

        public async Task<OfflineOrder> GetOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<OfflineOrder>().GetByIdAsync(id);
            _unitOfWork.ClearTracker();

            return order;
        }

        public async Task<OfflineOrder> UpdateStatus(OfflineOrder order, int statusId)
        {
            var orderStatus = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(statusId);

            if (orderStatus == null) return null;

            order.OrderStatus = orderStatus;

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return order;
        }

        public async Task<Order> UpdateWebsiteOrderStatus(Order order, int statusId)
        {
            var orderStatus = await _unitOfWork.Repository<OfflineOrderStatus>().GetByIdAsync(statusId);

            if (orderStatus == null) return null;

            order.OrderStatus = orderStatus;

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return order;
        }

        public async Task<OfflineOrder> GetOrderWithStatusAsync(int orderId)
        {
            var order = await _context.OfflineOrders.Include(o => o.OrderStatus).Where(o => o.Id == orderId).SingleOrDefaultAsync();

            return order;
        }

        public async Task<Order> GetWebsiteOrderWithStatusAsync(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderStatus).Where(o => o.Id == orderId).SingleOrDefaultAsync();

            return order;
        }

        public async Task<List<OfflineOrder>> GetOrdersByStatusIdAsync(int statusId)
        {
            var orders = await _context.OfflineOrders.Include(o => o.OrderStatus).Where(o => o.OrderStatus.Id == statusId).ToListAsync();

            return orders;
        }
    }
}