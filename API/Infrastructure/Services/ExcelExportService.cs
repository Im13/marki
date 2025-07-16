using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using OfficeOpenXml;

namespace Infrastructure.Services
{
    public class ExcelExportService : IExcelExportInterface
    {
        public byte[] ExportCustomersToExcel(IEnumerable<Customer> customers)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Marki");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "DOB";
                worksheet.Cells[1, 4].Value = "PhoneNumber";
                worksheet.Cells[1, 5].Value = "EmailAddress";

                int row = 2;
                foreach (var customer in customers)
                {
                    worksheet.Cells[row, 1].Value = customer.Id;
                    worksheet.Cells[row, 2].Value = customer.Name;
                    worksheet.Cells[row, 5].Value = customer.DOB;
                    worksheet.Cells[row, 3].Value = customer.PhoneNumber;
                    worksheet.Cells[row, 4].Value = customer.EmailAddress;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return package.GetAsByteArray();
            }
        }

        public byte[] ExportOrdersToExcel(IEnumerable<Order> orders)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Marki");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Customer";
                worksheet.Cells[1, 3].Value = "Phone";
                worksheet.Cells[1, 4].Value = "Total";
                worksheet.Cells[1, 5].Value = "Status";

                int row = 2;
                foreach (var order in orders)
                {
                    worksheet.Cells[row, 1].Value = order.Id;
                    worksheet.Cells[row, 2].Value = order.Fullname;
                    worksheet.Cells[row, 3].Value = order.PhoneNumber;
                    worksheet.Cells[row, 4].Value = order.Total;
                    worksheet.Cells[row, 5].Value = order.OrderStatus?.Status;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return package.GetAsByteArray();
            }
        }
    }
}