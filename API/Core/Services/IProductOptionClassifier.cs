using Core.Entities;

namespace Core.Services
{
    public interface IProductOptionClassifier
    {
         string ClassifyOption(string optionName);
        Dictionary<string, List<ProductOptionValues>> GroupOptionsByType(Product product);
    }
}