using Core.Entities;
using Core.Services;

namespace Infrastructure.Services.Recommendations
{
    public class ProductOptionClassifier : IProductOptionClassifier
    {
        private readonly Dictionary<string, List<string>> _optionKeywords;
        private readonly Dictionary<string, string> _cache;

        public ProductOptionClassifier()
        {
            _cache = new Dictionary<string, string>();
            _optionKeywords = new Dictionary<string, List<string>>
            {
                { "Color", new List<string> { "color", "colour", "màu", "mau", "màu sắc", "mau sac" } },
                { "Size", new List<string> { "size", "kích thước", "kich thuoc", "kích cỡ", "kich co", "size áo", "size quần" } },
                { "Material", new List<string> { "material", "chất liệu", "chat lieu", "vải", "vai" } },
                { "Style", new List<string> { "style", "phong cách", "phong cach", "kiểu dáng", "kieu dang" } },
                { "Pattern", new List<string> { "pattern", "họa tiết", "hoa tiet", "mẫu", "mau" } }
            };
        }

        public string ClassifyOption(string optionName)
        {
            if (string.IsNullOrWhiteSpace(optionName))
                return "Other";

            var normalized = NormalizeText(optionName);

            if (_cache.ContainsKey(normalized))
                return _cache[normalized];

            foreach (var kvp in _optionKeywords)
            {
                foreach (var keyword in kvp.Value)
                {
                    if (normalized.Contains(NormalizeText(keyword)))
                    {
                        _cache[normalized] = kvp.Key;
                        return kvp.Key;
                    }
                }
            }

            _cache[normalized] = "Other";
            return "Other";
        }

        public Dictionary<string, List<ProductOptionValues>> GroupOptionsByType(Product product)
        {
            var grouped = new Dictionary<string, List<ProductOptionValues>>();

            if (product.ProductOptions == null)
                return grouped;

            foreach (var option in product.ProductOptions)
            {
                var type = ClassifyOption(option.OptionName);

                if (!grouped.ContainsKey(type))
                    grouped[type] = new List<ProductOptionValues>();

                if (option.ProductOptionValues != null)
                    grouped[type].AddRange(option.ProductOptionValues);
            }

            return grouped;
        }

        private string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return RemoveVietnameseAccents(text.ToLower().Trim());
        }

        private string RemoveVietnameseAccents(string text)
        {
            var accents = new[]
            {
                new[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ" },
                new[] { "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ" },
                new[] { "í", "ì", "ỉ", "ĩ", "ị" },
                new[] { "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ" },
                new[] { "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự" },
                new[] { "ý", "ỳ", "ỷ", "ỹ", "ỵ" },
                new[] { "đ" }
            };

            var replacements = new[] { "a", "e", "i", "o", "u", "y", "d" };

            for (int i = 0; i < accents.Length; i++)
            {
                foreach (var accent in accents[i])
                {
                    text = text.Replace(accent, replacements[i]);
                }
            }

            return text;
        }
    }
}