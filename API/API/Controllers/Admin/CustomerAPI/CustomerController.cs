using API.Controllers;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification.CustomerSpec;
using Core.Specification.CustomerSpecification;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class CustomerController : BaseApiController
    {
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;
        public CustomerController(IGenericRepository<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
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
    }
}