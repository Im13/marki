using API.DTOs.Shopee;
using API.Errors;
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

        public ShopeeController(IShopeeOrderService shopeeOrderService, IMapper mapper)
        {
            _shopeeOrderService = shopeeOrderService;
            _mapper = mapper;
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders(List<ShopeeOrderDTO> orders)
        {
            var shopeeOrders = _mapper.Map<List<ShopeeOrderDTO>,List<ShopeeOrder>>(orders);

            var addedOrders = await _shopeeOrderService.CreateOrdersAsync(shopeeOrders);

            if(addedOrders == null) 
                return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(addedOrders);
        }

        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders([FromQuery]ShopeeOrderSpecParams productParams)
        {
            return Ok();
        }
    }
}