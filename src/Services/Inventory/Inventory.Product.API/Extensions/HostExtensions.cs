using Inventory.Product.API.Persistance;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<DatabaseSettings>();
            
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("Database setting is not configure");

            var mongoClient = services.GetRequiredService<IMongoClient>();
            new InventoryDbSeed()
               .SeedDataAsync(mongoClient, settings).Wait();
            return host;
        }
    }
}
