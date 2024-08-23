using API.Controllers;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification.CustomerSpec;
using Core.Specification.CustomerSpecification;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class CustomerController : BaseApiController
    {
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        public CustomerController(IGenericRepository<Customer> customerRepo, IMapper mapper, ICustomerService customerService)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomers([FromQuery] CustomerSpecParams customerParams)
        {
            var spec = new CustomerSpecification(customerParams);

            var countSpec = new CustomerWithFiltersForCountSpecification(customerParams);

            var totalItems = await _customerRepo.CountAsync(countSpec);

            var customers = await _customerRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<CustomerDTO>>(customers);

            return Ok(new Pagination<CustomerDTO>(customerParams.PageIndex, customerParams.PageSize, totalItems, data));
        }

        [HttpPost("delete-customers")]
        public async Task<ActionResult> DeleteCustomers(List<int> customerIds)
        {
            if (customerIds.Count <= 0)
                return BadRequest("No data received!");

            var deletedResult = await _customerService.DeleteCustomers(customerIds);

            if (!deletedResult) return BadRequest("Failed to delete!");

            return Ok();
        }
    }
}