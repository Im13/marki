using Core.Entities;
using Core.Entities.ShopeeOrder;
using Core.Interfaces;
using Infrastructure.Const;

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
                var tempOrd = allOrders.SingleOrDefault(o => o.OrderId == order.OrderId);

                if (tempOrd == null)
                {
                    _unitOfWork.Repository<ShopeeOrder>().Add(order);
                    addedList.Add(order);
                }
                else
                {
                    //Edit shopee order
                    tempOrd.ShipmentCode = order.ShipmentCode;
                    tempOrd.OrderStatus = order.OrderStatus;
                    _unitOfWork.Repository<ShopeeOrder>().Update(tempOrd);
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
                if (order.OrderStatus != OrderStatusConst.IS_CANCELED)
                {
                    foreach (var product in order.Products)
                    {
                        string orderId = "";
                        decimal revenue = 0;
                        decimal importPrice = 0;
                        decimal profit = 0;

                        if (products.Count(p => p.ProductSKU == product.SKU) > 0)
                            importPrice = products
                                .Where(p => p.ProductSKU == product.SKU)
                                .FirstOrDefault().ProductSKUs.First().ImportPrice;

                        if (!ordProducts.Any(o => o.OrderId == order.OrderId))
                        {
                            orderId = order.OrderId;
                            revenue = orderRevenueDict[order.OrderId] - order.ServiceFee - order.PaymentFee - order.ShopVoucher;
                            profit = revenue - importPrice * product.Quantity;
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
                            Revenue = revenue,
                            ProductSKU = product.SKU
                        });
                    }
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