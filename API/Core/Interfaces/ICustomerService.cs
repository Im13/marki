namespace Core.Interfaces
{
    public interface ICustomerService
    {
         Task<bool> DeleteCustomers(List<int> customerIds);
    }
}