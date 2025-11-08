using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Entities.Recommendation;
using Core.Entities.ShopeeOrder;

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
                context.ProductTypes.AddRange(types);
            }

            if (!context.Products.Any())
            {
                var productsData = File
                    .ReadAllText(path + @"/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData, _jsonOptions);
                context.Products.AddRange(products);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File
                    .ReadAllText(path + @"/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData, _jsonOptions);
                context.DeliveryMethods.AddRange(methods);
            }

            if (!context.Provinces.Any())
            {
                var provinceData = File
                    .ReadAllText(path + @"/Data/SeedData/province.json");
                var provinces = JsonSerializer.Deserialize<List<Province>>(provinceData, _jsonOptions);
                context.Provinces.AddRange(provinces);
            }

            if (!context.Districts.Any())
            {
                var districtData = File
                    .ReadAllText(path + @"/Data/SeedData/district.json");
                var districts = JsonSerializer.Deserialize<List<District>>(districtData);
                context.Districts.AddRange(districts);
            }

            if (!context.Wards.Any())
            {
                var wardData = File
                    .ReadAllText(path + @"/Data/SeedData/ward.json");
                var wards = JsonSerializer.Deserialize<List<Ward>>(wardData, _jsonOptions);
                context.Wards.AddRange(wards);
            }

            if (!context.Customers.Any())
            {
                var customersData = File
                    .ReadAllText(path + @"/Data/SeedData/customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customersData, _jsonOptions);
                context.Customers.AddRange(customers);
            }

            if (!context.OrderStatus.Any())
            {
                var orderStatusData = File
                    .ReadAllText(path + @"/Data/SeedData/orderStatuses.json");
                var statuses = JsonSerializer.Deserialize<List<OfflineOrderStatus>>(orderStatusData, _jsonOptions);
                context.OrderStatus.AddRange(statuses);
            }

            if (!context.SlideImages.Any())
            {
                var slideImagesData = File
                    .ReadAllText(path + @"/Data/SeedData/slideimages.json");
                var slideImages = JsonSerializer.Deserialize<List<SlideImage>>(slideImagesData, _jsonOptions);
                context.SlideImages.AddRange(slideImages);
            }

            if (!context.RevenueSummaries.Any())
            {
                var revenueSummariesData = File
                    .ReadAllText(path + @"/Data/SeedData/revenuesummaries.json");
                var revenueSummaries = JsonSerializer.Deserialize<List<RevenueSummary>>(revenueSummariesData, _jsonOptions);
                context.RevenueSummaries.AddRange(revenueSummaries);
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
                context.ProductOptions.AddRange(productOptions);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            if (!context.ProductOptionValues.Any())
            {
                var productOptionValuesData = File
                    .ReadAllText(path + @"/Data/SeedData/productoptionvalues.json");
                var productOptionValues = JsonSerializer.Deserialize<List<ProductOptionValues>>(productOptionValuesData, _jsonOptions);
                context.ProductOptionValues.AddRange(productOptionValues);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            if (!context.ProductSKUs.Any())
            {
                var productSkusData = File
                    .ReadAllText(path + @"/Data/SeedData/productskus.json");
                var productSkus = JsonSerializer.Deserialize<List<ProductSKUs>>(productSkusData, _jsonOptions);
                context.ProductSKUs.AddRange(productSkus);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            if (!context.ProductSKUValues.Any())
            {
                var productSkuValuesData = File
                    .ReadAllText(path + @"/Data/SeedData/productskuvalues.json");
                var productSkuValues = JsonSerializer.Deserialize<List<ProductSKUValues>>(productSkuValuesData, _jsonOptions);
                context.ProductSKUValues.AddRange(productSkuValues);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            if (!context.Photos.Any())
            {
                var photosData = File
                    .ReadAllText(path + @"/Data/SeedData/photos.json");
                var photos = JsonSerializer.Deserialize<List<Photo>>(photosData, _jsonOptions);
                context.Photos.AddRange(photos);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed Offline Orders (depends on Customers, Districts, Provinces, Wards, OrderStatus)
            if (!context.OfflineOrders.Any())
            {
                var offlineOrdersData = File
                    .ReadAllText(path + @"/Data/SeedData/offlineorders.json");
                var offlineOrders = JsonSerializer.Deserialize<List<OfflineOrder>>(offlineOrdersData, _jsonOptions);
                context.OfflineOrders.AddRange(offlineOrders);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed OfflineOrderSKUs (depends on OfflineOrders and ProductSKUs)
            if (!context.OfflineOrderSKUs.Any())
            {
                var offlineOrderSkusData = File
                    .ReadAllText(path + @"/Data/SeedData/offlineorderskus.json");
                var offlineOrderSkus = JsonSerializer.Deserialize<List<OfflineOrderSKUs>>(offlineOrderSkusData, _jsonOptions);
                context.OfflineOrderSKUs.AddRange(offlineOrderSkus);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed Shopee Orders (no dependencies for parent)
            if (!context.ShopeeOrders.Any())
            {
                var shopeeOrdersData = File
                    .ReadAllText(path + @"/Data/SeedData/shopeeorders.json");
                var shopeeOrders = JsonSerializer.Deserialize<List<ShopeeOrder>>(shopeeOrdersData, _jsonOptions);
                context.ShopeeOrders.AddRange(shopeeOrders);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed Shopee Products (depends on ShopeeOrders)
            if (!context.ShopeeProducts.Any())
            {
                var shopeeProductsData = File
                    .ReadAllText(path + @"/Data/SeedData/shopeeproducts.json");
                var shopeeProducts = JsonSerializer.Deserialize<List<ShopeeProduct>>(shopeeProductsData, _jsonOptions);
                context.ShopeeProducts.AddRange(shopeeProducts);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
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
                context.UserSessions.AddRange(userSessions);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // Seed SessionInteractions (depends on UserSessions and Products)
            // Note: Only seed if file exists
            var sessionInteractionsPath = path + @"/Data/SeedData/sessioninteractions.json";
            if (!context.SessionInteractions.Any() && File.Exists(sessionInteractionsPath))
            {
                var sessionInteractionsData = File.ReadAllText(sessionInteractionsPath);
                var sessionInteractions = JsonSerializer.Deserialize<List<SessionInteraction>>(sessionInteractionsData, _jsonOptions);
                context.SessionInteractions.AddRange(sessionInteractions);
            }

            if (context.ChangeTracker.HasChanges())
            {
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
    }
}
