using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("provinces")]
        public async Task<IReadOnlyList<Province>> GetProvinces()
        {
            return await _addressService.GetProvinces();
        }

        [HttpGet("districts")]
        public async Task<IReadOnlyList<District>> GetDistricts()
        {
            return await _addressService.GetDistricts();
        }

        [HttpGet("wards")]
        public async Task<IReadOnlyList<Ward>> GetWards()
        {
            return await _addressService.GetWards();
        }

        [HttpGet("districts/{id}")]
        public async Task<IReadOnlyList<District>> GetDistrictsByProvinceID(int id)
        {
            return await _addressService.GetDistrictByProvinceId(id);
        }

        [HttpGet("wards/{id}")]
        public async Task<IReadOnlyList<Ward>> GetWardsByDistrictId(int id)
        {
            return await _addressService.GetWardByDistricId(id);
        }
    }
}