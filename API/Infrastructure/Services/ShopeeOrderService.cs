using Core.Entities;
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

            if (orders.Count == 0)
                return addedList;

            var allOrders = await _unitOfWork.Repository<ShopeeOrder>().ListAllAsync();

            foreach (var order in orders)
            {
                if (allOrders.FirstOrDefault(o => o.OrderId == order.OrderId) == null)
                {
                    _unitOfWork.Repository<ShopeeOrder>().Add(order);
                    addedList.Add(order);
                }
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return addedList;
        }

        public async Task<List<ShopeeOrderProducts>> GetOrderProductsStatistic(List<ShopeeOrder> orders)
        {
            List<ShopeeOrderProducts> ordProducts = new List<ShopeeOrderProducts>();
            var productDetails = new Product();

            // Need improvement later
            var products = await _unitOfWork.Repository<Product>().ListAllAsync();

            Dictionary<string, decimal> orderRevenueDict = GetAllOrdersRevenue(orders);

            foreach (var order in orders)
            {
                foreach (var product in order.Products)
                {
                    string orderId = "";
                    decimal revenue = 0;
                    decimal importPrice = 0;
                    decimal profit = 0;

                    if (products.Count(p => p.ProductSKU == product.SKU) > 0)
                        importPrice = products.Where(p => p.ProductSKU == product.SKU).FirstOrDefault().ImportPrice;

                    if (!ordProducts.Any(o => o.OrderId == order.OrderId))
                    {
                        orderId = order.OrderId;
                        revenue = orderRevenueDict[order.OrderId] - order.ServiceFee - order.PaymentFee - order.ShopVoucher;
                        profit = revenue - productDetails.ImportPrice * product.Quantity;
                    }
                    else
                    {
                        profit = 0 - importPrice * product.Quantity;
                    }

                    ordProducts.Add(new ShopeeOrderProducts()
                    {
                        ImportPrice = importPrice,
                        OrderId = orderId,
                        OrderProfit = profit,
                        OrderStatus = order.OrderStatus,
                        ProductName = product.ProductName,
                        ProductQuantity = product.Quantity,
                        Revenue = revenue
                    });
                }
            }

            return ordProducts;
        }

        public Dictionary<string, decimal> GetAllOrdersRevenue(List<ShopeeOrder> allOrders)
        {
            var orderRevenueDict = new Dictionary<string, decimal>();

            foreach (var order in allOrders)
            {
                foreach (var product in order.Products)
                {
                    if (orderRevenueDict.ContainsKey(order.OrderId))
                    {
                        orderRevenueDict[order.OrderId] += product.TotalSellingPrice;
                    }
                    else
                    {
                        orderRevenueDict.Add(order.OrderId, product.TotalSellingPrice);
                    }
                }
            }

            return orderRevenueDict;
        }
    }
}