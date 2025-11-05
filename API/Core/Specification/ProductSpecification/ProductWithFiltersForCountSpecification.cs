using Core.Entities;

namespace Core.Specification
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams) : base(x => 
            (string.IsNullOrEmpty(productParams.Search) || 
             x.Name.ToLower().Contains(productParams.Search) ||
             x.ProductSKUs.Any(sku => sku.SKU.ToLower().Contains(productParams.Search))) &&
            (x.IsDeleted == false) &&
            (productParams.TypeId == null || productParams.TypeId == 0 || x.ProductTypeId == productParams.TypeId) &&
            (string.IsNullOrEmpty(productParams.Sort) || productParams.Sort.ToLower() != "new-arrivals" || x.CreatedAt >= DateTime.UtcNow.AddDays(-14))
        )
        {
        }
    }
}