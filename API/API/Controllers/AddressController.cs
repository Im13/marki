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

        [Authorize(Roles = "Admin")]
        [HttpGet("provinces")]
        public async Task<IReadOnlyList<Province>> GetProvinces()
        {
            return await _addressService.GetProvinces();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("districts")]
        public async Task<IReadOnlyList<District>> GetDistricts()
        {
            return await _addressService.GetDistricts();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("wards")]
        public async Task<IReadOnlyList<Ward>> GetWards()
        {
            return await _addressService.GetWards();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("districts/{id}")]
        public async Task<IReadOnlyList<District>> GetDistrictsByProvinceID(int id)
        {
            return await _addressService.GetDistrictByProvinceId(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("wards/{id}")]
        public async Task<IReadOnlyList<Ward>> GetWardsByDistrictId(int id)
        {
            return await _addressService.GetWardByDistricId(id);
        }
    }
}