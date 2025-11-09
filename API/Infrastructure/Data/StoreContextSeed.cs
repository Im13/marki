using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Entities.Recommendation;
using Core.Entities.ShopeeOrder;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new NumericBooleanJsonConverter(),
                new FlexibleDateTimeConverter(),
                new JsonStringEnumConverter()
            }
        };

        public static async Task SeedAsync(StoreContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Seed base entities first (no dependencies)
            if (!context.ProductTypes.Any())
            {
                var typesData = File
                    .ReadAllText(path + @"/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.ProductTypes, types);
            }

            if (!context.Products.Any())
            {
                var productsData = File
                    .ReadAllText(path + @"/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.Products, products);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File
                    .ReadAllText(path + @"/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.DeliveryMethods, methods);
            }

            if (!context.Provinces.Any())
            {
                var provinceData = File
                    .ReadAllText(path + @"/Data/SeedData/province.json");
                var provinces = JsonSerializer.Deserialize<List<Province>>(provinceData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.Provinces, provinces);
            }

            if (!context.Districts.Any())
            {
                var districtData = File
                    .ReadAllText(path + @"/Data/SeedData/district.json");
                var districts = JsonSerializer.Deserialize<List<District>>(districtData);
                await AddRangeWithIdentityInsertAsync(context, context.Districts, districts);
            }

            if (!context.Wards.Any())
            {
                var wardData = File
                    .ReadAllText(path + @"/Data/SeedData/ward.json");
                var wards = JsonSerializer.Deserialize<List<Ward>>(wardData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.Wards, wards);
            }

            if (!context.Customers.Any())
            {
                var customersData = File
                    .ReadAllText(path + @"/Data/SeedData/customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customersData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.Customers, customers);
            }

            if (!context.OrderStatus.Any())
            {
                var orderStatusData = File
                    .ReadAllText(path + @"/Data/SeedData/orderStatuses.json");
                var statuses = JsonSerializer.Deserialize<List<OfflineOrderStatus>>(orderStatusData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.OrderStatus, statuses);
            }

            if (!context.SlideImages.Any())
            {
                var slideImagesData = File
                    .ReadAllText(path + @"/Data/SeedData/slideimages.json");
                var slideImages = JsonSerializer.Deserialize<List<SlideImage>>(slideImagesData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.SlideImages, slideImages);
            }

            if (!context.RevenueSummaries.Any())
            {
                var revenueSummariesData = File
                    .ReadAllText(path + @"/Data/SeedData/revenuesummaries.json");
                var revenueSummaries = JsonSerializer.Deserialize<List<RevenueSummary>>(revenueSummariesData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.RevenueSummaries, revenueSummaries);
            }

            // Save base entities before adding dependent entities
            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed Product-related entities (depends on Products)
            if (!context.ProductOptions.Any())
            {
                var productOptionsData = File
                    .ReadAllText(path + @"/Data/SeedData/productoptions.json");
                var productOptions = JsonSerializer.Deserialize<List<ProductOptions>>(productOptionsData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.ProductOptions, productOptions);
            }

            if (!context.ProductOptionValues.Any())
            {
                var productOptionValuesData = File
                    .ReadAllText(path + @"/Data/SeedData/productoptionvalues.json");
                var productOptionValueSeeds = JsonSerializer.Deserialize<List<ProductOptionValueSeed>>(productOptionValuesData, _jsonOptions);

                if (productOptionValueSeeds != null)
                {
                    var entities = new List<ProductOptionValues>();
                    foreach (var seed in productOptionValueSeeds)
                    {
                        var entity = new ProductOptionValues
                        {
                            Id = seed.Id,
                            ValueName = seed.ValueName,
                            ValueTempId = seed.ValueTempId
                        };

                        entities.Add(entity);
                        context.Entry(entity).Property<int?>("ProductOptionId").CurrentValue = seed.ProductOptionId;
                    }

                    await AddRangeWithIdentityInsertAsync(context, context.ProductOptionValues, entities);
                }
            }

            if (!context.ProductSKUs.Any())
            {
                var productSkusData = File
                    .ReadAllText(path + @"/Data/SeedData/productskus.json");
                var productSkus = JsonSerializer.Deserialize<List<ProductSKUs>>(productSkusData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.ProductSKUs, productSkus);
            }

            if (!context.ProductSKUValues.Any())
            {
                var productSkuValuesData = File
                    .ReadAllText(path + @"/Data/SeedData/productskuvalues.json");
                var productSkuValueSeeds = JsonSerializer.Deserialize<List<ProductSkuValueSeed>>(productSkuValuesData, _jsonOptions);

                if (productSkuValueSeeds != null)
                {
                    var entities = new List<ProductSKUValues>();
                    foreach (var seed in productSkuValueSeeds)
                    {
                        var entity = new ProductSKUValues
                        {
                            Id = seed.Id,
                            ValueTempId = seed.ValueTempId
                        };

                        entities.Add(entity);
                        context.Entry(entity).Property<int?>("ProductOptionValueId").CurrentValue = seed.ProductOptionValueId;
                        context.Entry(entity).Property<int?>("ProductSKUsId").CurrentValue = seed.ProductSKUsId;
                    }

                    await AddRangeWithIdentityInsertAsync(context, context.ProductSKUValues, entities);
                }
            }

            if (!context.Photos.Any())
            {
                var photosData = File
                    .ReadAllText(path + @"/Data/SeedData/photos.json");
                var photos = JsonSerializer.Deserialize<List<Photo>>(photosData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.Photos, photos);
            }

            // Seed Offline Orders (depends on Customers, Districts, Provinces, Wards, OrderStatus)
            if (!context.OfflineOrders.Any())
            {
                var offlineOrdersData = File
                    .ReadAllText(path + @"/Data/SeedData/offlineorders.json");
                var offlineOrders = JsonSerializer.Deserialize<List<OfflineOrder>>(offlineOrdersData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.OfflineOrders, offlineOrders);
            }

            // Seed OfflineOrderSKUs (depends on OfflineOrders and ProductSKUs)
            if (!context.OfflineOrderSKUs.Any())
            {
                var offlineOrderSkusData = File
                    .ReadAllText(path + @"/Data/SeedData/offlineorderskus.json");
                var offlineOrderSkus = JsonSerializer.Deserialize<List<OfflineOrderSKUs>>(offlineOrderSkusData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.OfflineOrderSKUs, offlineOrderSkus);
            }

            // Seed Shopee Orders (no dependencies for parent)
            if (!context.ShopeeOrders.Any())
            {
                var shopeeOrdersData = File
                    .ReadAllText(path + @"/Data/SeedData/shopeeorders.json");
                var shopeeOrders = JsonSerializer.Deserialize<List<ShopeeOrder>>(shopeeOrdersData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.ShopeeOrders, shopeeOrders);
            }

            // Seed Shopee Products (depends on ShopeeOrders)
            if (!context.ShopeeProducts.Any())
            {
                var shopeeProductsData = File
                    .ReadAllText(path + @"/Data/SeedData/shopeeproducts.json");
                var shopeeProducts = JsonSerializer.Deserialize<List<ShopeeProduct>>(shopeeProductsData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.ShopeeProducts, shopeeProducts);
            }

            // Seed Notifications (no dependencies)
            if (!context.Notifications.Any())
            {
                var notificationsData = File
                    .ReadAllText(path + @"/Data/SeedData/notifications.json");
                var notifications = JsonSerializer.Deserialize<List<Notification>>(notificationsData, _jsonOptions);
                context.Notifications.AddRange(notifications);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed NotificationUsers (depends on Notifications)
            if (!context.NotificationUsers.Any())
            {
                var notificationUsersData = File
                    .ReadAllText(path + @"/Data/SeedData/notificationusers.json");
                var notificationUsers = JsonSerializer.Deserialize<List<NotificationUser>>(notificationUsersData, _jsonOptions);
                context.NotificationUsers.AddRange(notificationUsers);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed UserSessions (no dependencies)
            if (!context.UserSessions.Any())
            {
                var userSessionsData = File
                    .ReadAllText(path + @"/Data/SeedData/usersessions.json");
                var userSessions = JsonSerializer.Deserialize<List<UserSession>>(userSessionsData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.UserSessions, userSessions);
            }

            // Seed SessionInteractions (depends on UserSessions and Products)
            // Note: Only seed if file exists
            var sessionInteractionsPath = path + @"/Data/SeedData/sessioninteractions.json";
            if (!context.SessionInteractions.Any() && File.Exists(sessionInteractionsPath))
            {
                var sessionInteractionsData = File.ReadAllText(sessionInteractionsPath);
                var sessionInteractions = JsonSerializer.Deserialize<List<SessionInteraction>>(sessionInteractionsData, _jsonOptions);
                await AddRangeWithIdentityInsertAsync(context, context.SessionInteractions, sessionInteractions);
            }
        }

        private static bool IsSqlServer(StoreContext context) =>
            context.Database.ProviderName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) == true;

        private static async Task AddRangeWithIdentityInsertAsync<TEntity>(
            StoreContext context,
            DbSet<TEntity> dbSet,
            IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            var requiresIdentityInsert = IsSqlServer(context) && entities.Any(e => e.Id > 0);
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            if (entityType == null)
            {
                throw new InvalidOperationException($"Unable to determine table mapping for entity type {typeof(TEntity).Name}");
            }

            var tableName = entityType.GetTableName();
            var schema = entityType.GetSchema() ?? "dbo";

            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} does not have a mapped table name.");
            }

            var fullTableName = $"[{schema}].[{tableName}]";

            if (requiresIdentityInsert)
            {
                await using var transaction = await context.Database.BeginTransactionAsync();
                await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {fullTableName} ON");
                dbSet.AddRange(entities);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {fullTableName} OFF");
                await transaction.CommitAsync();
            }
            else
            {
                dbSet.AddRange(entities);
                await context.SaveChangesAsync();
            }
        }

        private class NumericBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.TokenType switch
                {
                    JsonTokenType.True => true,
                    JsonTokenType.False => false,
                    JsonTokenType.Number => reader.GetDouble() != 0,
                    JsonTokenType.String => ParseString(reader.GetString()),
                    _ => throw new JsonException($"Unexpected token parsing boolean. Token: {reader.TokenType}")
                };
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                writer.WriteBooleanValue(value);
            }

            private static bool ParseString(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;

                if (bool.TryParse(value, out var boolValue))
                {
                    return boolValue;
                }

                if (double.TryParse(value, out var numberValue))
                {
                    return numberValue != 0;
                }

                throw new JsonException($"Unable to convert \"{value}\" to boolean.");
            }
        }

        private class FlexibleDateTimeConverter : JsonConverter<DateTime>
        {
            private static readonly string[] SupportedFormats =
            {
                "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd",
                "MM/dd/yyyy HH:mm:ss",
                "MM/dd/yyyy"
            };

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    var raw = reader.GetString();
                    if (string.IsNullOrWhiteSpace(raw))
                    {
                        return default;
                    }

                    if (DateTime.TryParseExact(raw, SupportedFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
                    {
                        return parsed;
                    }

                    if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out parsed))
                    {
                        return parsed;
                    }

                    throw new JsonException($"Unable to convert \"{raw}\" to DateTime.");
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    if (reader.TryGetInt64(out var ticks))
                    {
                        return new DateTime(ticks, DateTimeKind.Utc);
                    }
                }

                throw new JsonException($"Unexpected token parsing DateTime. Token: {reader.TokenType}");
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
            }
        }

        private class ProductOptionValueSeed
        {
            public int Id { get; set; }
            public string ValueName { get; set; }
            public int ValueTempId { get; set; }
            public int? ProductOptionId { get; set; }
        }

        private class ProductSkuValueSeed
        {
            public int Id { get; set; }
            public int ValueTempId { get; set; }
            public int? ProductOptionValueId { get; set; }
            public int? ProductSKUsId { get; set; }
        }
    }
}
