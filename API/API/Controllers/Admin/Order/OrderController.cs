using API.DTOs.AdminOrder;
using API.Errors;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin.Order
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
            
        }

        [HttpPost("create")]
        public async Task<ActionResult<OfflineOrderDTO>> CreateOrder(OfflineOrderDTO orderDTO)
        {
            if(orderDTO == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            var orderToCreate = _mapper.Map<OfflineOrderDTO,OfflineOrder>(orderDTO);

            var orderCreated = await _orderService.CreateOfflineOrder(orderToCreate);

            if (orderCreated == null) return BadRequest("Error create order");

            return Ok(_mapper.Map<OfflineOrder,OfflineOrderDTO>(orderCreated));
        }
    }
}