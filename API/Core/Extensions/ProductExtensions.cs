using Core.Entities;

namespace Core.Extensions
{
    public static class ProductExtensions
    {
        public static bool HasStock(this Product product)
        {
            return product.ProductSKUs?.Any(sku => sku.Quantity > 0) ?? false;
        }

        public static int TotalStock(this Product product)
        {
            return product.ProductSKUs?.Sum(sku => sku.Quantity) ?? 0;
        }

        public static (decimal minPrice, decimal maxPrice) GetPriceRange(this Product product)
        {
            if (product.ProductSKUs == null || !product.ProductSKUs.Any())
                return (product.ImportPrice, product.ImportPrice);

            var prices = product.ProductSKUs.Select(sku => sku.Price).ToList();
            return (prices.Min(), prices.Max());
        }

        public static string GetPriceDisplay(this Product product)
        {
            var (minPrice, maxPrice) = product.GetPriceRange();

            return minPrice == maxPrice
                ? $"{minPrice:N0}đ"
                : $"{minPrice:N0}đ - {maxPrice:N0}đ";
        }

        public static decimal GetRepresentativePrice(this Product product)
        {
            if (product.ProductSKUs == null || !product.ProductSKUs.Any())
                return product.ImportPrice;

            return product.ProductSKUs.Min(sku => sku.Price);
        }

        public static List<string> GetColors(this Product product)
        {
            var colorOption = product.ProductOptions?
                .FirstOrDefault(opt => ClassifyOption(opt.OptionName) == "Color");

            return colorOption?.ProductOptionValues?.Select(v => v.ValueName).Distinct().ToList()
                ?? new List<string>();
        }

        public static List<string> GetAvailableSizes(this Product product)
        {
            var sizeOption = product.ProductOptions?
                .FirstOrDefault(opt => ClassifyOption(opt.OptionName) == "Size");

            return sizeOption?.ProductOptionValues?.Select(v => v.ValueName).Distinct().ToList()
                ?? new List<string>();
        }

        public static List<string> GetMaterials(this Product product)
        {
            var materialOption = product.ProductOptions?
                .FirstOrDefault(opt => ClassifyOption(opt.OptionName) == "Material");

            return materialOption?.ProductOptionValues?.Select(v => v.ValueName).Distinct().ToList()
                ?? new List<string>();
        }

        private static string ClassifyOption(string optionName)
        {
            if (string.IsNullOrWhiteSpace(optionName))
                return "Other";

            var normalized = optionName.ToLower().Trim();

            if (normalized.Contains("color") || normalized.Contains("màu"))
                return "Color";
            if (normalized.Contains("size") || normalized.Contains("kích") || normalized.Contains("cỡ"))
                return "Size";
            if (normalized.Contains("material") || normalized.Contains("chất") || normalized.Contains("liệu"))
                return "Material";

            return "Other";
        }
    }
}