using API.DTOs.AdminOrder;
using API.Helpers;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification.OfflineOrderSpec;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin.Order
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<OfflineOrder> _offlineOrderRepo;
        
        public OrderController(IOrderService orderService, IMapper mapper, IOrderRepository orderRepository, IGenericRepository<OfflineOrder> offlineOrderRepo)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _offlineOrderRepo = offlineOrderRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<OfflineOrderDTO>>> GetOrders([FromQuery] OrderSpecParams orderParams)
        {
            var spec = new OrderSpecification(orderParams);

            var countSpec = new OrdersWithFiltersForCountSpecification(orderParams);

            var totalItems = await _offlineOrderRepo.CountAsync(countSpec);

            var orders = await _orderRepository.GetOrdersWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<OfflineOrder>, IReadOnlyList<OfflineOrderDTO>>(orders);

            return Ok(new Pagination<OfflineOrderDTO>(orderParams.PageIndex, orderParams.PageSize, totalItems, data));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder(OfflineOrderDTO orderDTO)
        {
            if(orderDTO == null) return BadRequest();

            //Find order by id
            var order = await _orderService.GetOrderAsync(orderDTO.Id);
            if(order == null) return BadRequest("Order not exists");

            var orderToUpdate = _mapper.Map<OfflineOrderDTO,OfflineOrder>(orderDTO);
            var currentListSkus = _mapper.Map<List<OfflineOrderSKUDTOs>,List<OfflineOrderSKUs>>(orderDTO.OfflineOrderSKUs);

            var orderUpdateResult = await _orderService.UpdateOfflineOrder(orderToUpdate, currentListSkus);

            if (orderUpdateResult == null) return BadRequest("Error update order");

            return Ok();
        }

        [HttpGet("status")]
        public async Task<ActionResult<Pagination<OfflineOrderDTO>>> GetOrderByStatusId([FromQuery] OrderSpecParams orderParams)
        {
            var spec = new OrderWithStatusFilterSpecification(orderParams);

            var countSpec = new OrdersWithStatusFilterForCountSpecification(orderParams);

            var totalItems = await _offlineOrderRepo.CountAsync(countSpec);

            var orders = await _orderRepository.GetOrdersWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<OfflineOrder>, IReadOnlyList<OfflineOrderDTO>>(orders);

            return Ok(new Pagination<OfflineOrderDTO>(orderParams.PageIndex, orderParams.PageSize, totalItems, data));
        }
    }
}