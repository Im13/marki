using API.DTOs.Shopee;
using AutoMapper;
using Core.Entities.ShopeeOrder;
using Core.Interfaces;
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

            var updateResult = await _shopeeOrderService.CreateOrdersAsync(shopeeOrders);

            if(updateResult) 
                return Ok();

            return BadRequest("Problem create orders!");
        }
    }
}