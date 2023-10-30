namespace Core.Entities 
{
    public interface IAddressService
    {
        Task<IReadOnlyList<Province>> GetProvinces();
        Task<IReadOnlyList<Ward>> GetWards();
        Task<IReadOnlyList<District>> GetDistricts();
        Task<IReadOnlyList<District>> GetDistrictByProvinceId(int id);
        Task<IReadOnlyList<Ward>> GetWardByDistricId(int id);
    }
}