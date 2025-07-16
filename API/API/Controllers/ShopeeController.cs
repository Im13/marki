using API.DTOs.Shopee;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.ShopeeOrder;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShopeeController : BaseApiController
    {
        private readonly IShopeeOrderService _shopeeOrderService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ShopeeOrder> _shopeeOrderRepo;

        public ShopeeController(IShopeeOrderService shopeeOrderService, IMapper mapper, IGenericRepository<ShopeeOrder> shopeeOrderRepo)
        {
            _shopeeOrderService = shopeeOrderService;
            _mapper = mapper;
            _shopeeOrderRepo = shopeeOrderRepo;
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders(List<ShopeeOrderDTO> orders)
        {
            var shopeeOrders = _mapper.Map<List<ShopeeOrderDTO>, List<ShopeeOrder>>(orders);

            var addedOrders = await _shopeeOrderService.CreateOrdersAsync(shopeeOrders);

            if (addedOrders == null)
                return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(addedOrders);
        }

        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders([FromQuery] ShopeeOrderSpecParams shopeeOrderSpecParams)
        {
            var spec = new ShopeeOrderSpecification(shopeeOrderSpecParams);

            var countSpec = new ShopeeOrderWithFilterForCountSpecification(shopeeOrderSpecParams);

            var totalItems = await _shopeeOrderRepo.CountAsync(countSpec);

            var shopeOrders = await _shopeeOrderRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ShopeeOrder>, IReadOnlyList<ShopeeOrderDTO>>(shopeOrders);

            return Ok(new Pagination<ShopeeOrderDTO>(shopeeOrderSpecParams.PageIndex, shopeeOrderSpecParams.PageSize, totalItems, data));
        }

        [HttpGet("get-orders-from-shopee")]
        public async Task<IActionResult> GetOrdersFromShopee()
        {
            return Ok();
        }

        [HttpGet("statistic/get-orders")]
        public async Task<List<ShopeeOrderProducts>> GetOrdersStatistic([FromQuery] ShopeeOrderSpecParams shopeeOrderSpecParams)
        {
            var spec = new ShopeeProductsInOrdersSpecification(shopeeOrderSpecParams);

            var shopeeOrders = await _shopeeOrderRepo.ListAsync(spec);

            var orderProducts = await _shopeeOrderService.GetOrderProductsStatistic(shopeeOrders.ToList());

            return orderProducts;
        }
    }
}