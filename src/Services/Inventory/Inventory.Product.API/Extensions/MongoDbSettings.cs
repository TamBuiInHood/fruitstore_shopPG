using Shared.Configurations;

namespace Inventory.Product.API.Extensions
{
    public class MongoDbSettings : Shared.Configurations.DatabaseSettings
    {
        public string DatabaseName { get; set; }
    }
}
