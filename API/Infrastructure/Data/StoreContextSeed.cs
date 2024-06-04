using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            // if (!context.Products.Any())
            // {
            //     var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
            //     var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            //     context.Products.AddRange(products);
            // }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                context.DeliveryMethods.AddRange(methods);
            }

            if(!context.Provinces.Any())
            {
                var provinceData = File.ReadAllText("../Infrastructure/Data/SeedData/province.json");
                var provinces = JsonSerializer.Deserialize<List<Province>>(provinceData);;
                context.Provinces.AddRange(provinces);
            }

            if(!context.Districts.Any())
            {
                var districtData = File.ReadAllText("../Infrastructure/Data/SeedData/district.json");
                var districts = JsonSerializer.Deserialize<List<District>>(districtData);;
                context.Districts.AddRange(districts);
            }

            if(!context.Wards.Any())
            {
                var wardData = File.ReadAllText("../Infrastructure/Data/SeedData/ward.json");
                var wards = JsonSerializer.Deserialize<List<Ward>>(wardData);;
                context.Wards.AddRange(wards);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}