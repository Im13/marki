using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Recommendations;
using Core.Services;
using Infrastructure.BackgroundJobs;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Infrastructure.Services.Recommendations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient();
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRevenueSummaryRepository, RevenueSummaryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISlideImageRepository, SlideImageRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IRevenueSummaryService, RevenueSummaryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IWebSettingServices, WebSettingServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShopeeOrderService, ShopeeOrderService>();
            services.AddScoped<IExcelExportInterface, ExcelExportService>();
            services.AddScoped<IFacebookMarketingService, FacebookMarketingService>();
            services.AddScoped<INotificationService, NotificationService>();

            // Repositories
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISessionInteractionRepository, SessionInteractionRepository>();
            services.AddScoped<IProductCoOccurrenceRepository, ProductCoOccurrenceRepository>();
            services.AddScoped<IProductTrendingRepository, ProductTrendingRepository>();
            services.AddScoped<IRecommendationRepository, RecommendationRepository>();

            // Services
            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<IContentBasedRecommender, ContentBasedRecommender>();
            services.AddScoped<ISessionBasedRecommender, SessionBasedRecommender>();
            services.AddScoped<IPopularityBasedRecommender, PopularityBasedRecommender>();
            services.AddSingleton<IProductOptionClassifier, ProductOptionClassifier>();

            // Background Jobs
            services.AddHostedService<CoOccurrenceUpdateJob>();
            services.AddHostedService<TrendingUpdateJob>();
            services.AddHostedService<SessionCleanupJob>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.Configure<FacebookSettings>(config.GetSection("FacebookSettings"));
            services.AddSignalR();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Error = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4201", "https://localhost:4200");
                });
            });

            return services;
        }
    }
}