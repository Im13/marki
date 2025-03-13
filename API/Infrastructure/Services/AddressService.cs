using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _uow;

        public AddressService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IReadOnlyList<District>> GetDistrictByProvinceId(int id)
        {
            var districts = await _uow.Repository<District>().ListAllAsync();

            return districts.Where(d => d.ProvinceId == id).ToList();
        }

        public async Task<IReadOnlyList<Ward>> GetWardByDistricId(int id)
        {
            var wards = await _uow.Repository<Ward>().ListAllAsync();

            return wards.Where(w => w.DistrictId == id).ToList();
        }

        public async Task<IReadOnlyList<District>> GetDistricts()
        {
            return await _uow.Repository<District>().ListAllAsync();
        }

        public async Task<IReadOnlyList<Province>> GetProvinces()
        {
            return await _uow.Repository<Province>().ListAllAsync();
        }

        public async Task<IReadOnlyList<Ward>> GetWards()
        {
            return await _uow.Repository<Ward>().ListAllAsync();
        }
    }
}