using System.Security.Claims;
using API.DTOs;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification.OfflineOrderSpec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Order> _orderRepo;
        public OrdersController(IOrderService orderService, IMapper mapper, IGenericRepository<Order> orderRepo, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderRepo = orderRepo;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDTO,Address>(orderDTO.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, orderDTO.BasketId, address);
            
            if(order == null) return BadRequest(new ApiResponse(400, "Problem creating order")); 

            return Ok(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if(order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<OrderToReturnDTO>(order);
        }

        [Authorize]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        // [Authorize]
        [HttpGet("all-orders")]
        public async Task<ActionResult<Pagination<OrderToReturnDTO>>> GetOrders([FromQuery] OrderSpecParams orderParams)
        {
            var spec = new WebsiteOrderSpecification(orderParams);

            var countSpec = new WebsiteOrdersWithFiltersForCountSpecification(orderParams);

            var totalItems = await _orderRepo.CountAsync(countSpec);

            var orders = await _orderRepository.GetWebsiteOrdersWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders);

            return Ok(new Pagination<OrderToReturnDTO>(orderParams.PageIndex, orderParams.PageSize, totalItems, data));
        }

        // [Authorize]
        [HttpGet("website/{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetById(int id)
        {
            var order = await _orderRepository.GetWebsiteOrderById(id);

            return _mapper.Map<Order, OrderToReturnDTO>(order);
        }
    }
}