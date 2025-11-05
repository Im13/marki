using Core.Entities;

namespace Core.Specification
{
    public class ProductsWithTypesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesSpecification(ProductSpecParams productParams) : base(x => 
            (string.IsNullOrEmpty(productParams.Search) || 
             x.Name.ToLower().Contains(productParams.Search) ||
             x.ProductSKUs.Any(sku => sku.SKU.ToLower().Contains(productParams.Search))) &&
            (x.IsDeleted == false) &&
            (productParams.TypeId == null || productParams.TypeId == 0 || x.ProductTypeId == productParams.TypeId) &&
            (string.IsNullOrEmpty(productParams.Sort) || productParams.Sort.ToLower() != "new-arrivals" || x.CreatedAt >= DateTime.UtcNow.AddDays(-14))
        )
        {
            AddInclude(x => x.ProductType);
            
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort.ToLower())
                {
                    case "new-arrivals":
                        AddOrderByDescending(x => x.CreatedAt);
                        break;
                    case "priceasc":
                        // AddOrderBy(p => p.Price);
                        AddOrderByDescending(x => x.Id);
                        break;
                    case "pricedesc":
                        // AddOrderByDescending(p => p.Price);
                        AddOrderByDescending(x => x.Id);
                        break;
                    default:
                        AddOrderByDescending(x => x.Id);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(x => x.Id);
            }
            
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
        }

        public ProductsWithTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
        }
    }
}