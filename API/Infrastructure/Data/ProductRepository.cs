using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithSpec(ISpecification<Product> spec)
        {
            var products = await SpecificationEvaluator<Product>.GetQuery(_context.Set<Product>().AsQueryable(), spec)
                .Include(p => p.ProductSKUs).ThenInclude(ps => ps.ProductSKUValues)
                .Include(p => p.ProductSKUs).ThenInclude(ps => ps.Photos)
                .Include(p => p.ProductOptions).ThenInclude(po => po.ProductOptionValues)
                .ToListAsync();

            return products;
        }
    }
}