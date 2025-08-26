using Shared.Configurations;
using MongoDB.Driver;
using Infrastructure.Extensions;
using Inventory.Product.API.Services.Interfaces;
using Inventory.Product.API.Services;
namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtionsions
    {

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
          
            var databaseSetting = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
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
            var settings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
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
    }
}
