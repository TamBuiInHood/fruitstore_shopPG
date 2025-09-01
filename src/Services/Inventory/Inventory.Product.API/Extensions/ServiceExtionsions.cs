using Shared.Configurations;
using MongoDB.Driver;
using Infrastructure.Extensions;
using Inventory.Product.API.Services.Interfaces;
using Inventory.Product.API.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Inventory.Product.API.Consumer;
namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtionsions
    {

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
          
            var databaseSetting = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton(databaseSetting);

            return services;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(new MongoClient(GetMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
            
        }

        private static string GetMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
            if(settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("Database setting is not configure");

            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = settings.ConnectionString +  "/" + databaseName + "?authSource=admin";
            return mongoDbConnectionString;
        }
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            services.AddScoped<IInventoryServices, InventoryService>();
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var setting = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (String.IsNullOrEmpty(setting.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured.");

            var mqConnection = new Uri(setting.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    //cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckOutConsumer>(ctx);
                    //});
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
