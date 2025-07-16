using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IExcelExportInterface
    {
        byte[] ExportOrdersToExcel(IEnumerable<Order> orders);
        byte[] ExportCustomersToExcel(IEnumerable<Customer> customers);
    }
}