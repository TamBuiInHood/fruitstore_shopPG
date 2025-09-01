using Amazon.Runtime.Documents;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Extensions;
using MongoDB.Driver;

namespace Inventory.Product.API.Persistance
{
    public class InventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient mongoClient, MongoDbSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryEntries");
            if (await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(GetPreconfiguredInventoryEntries());
            }
        }

        private IEnumerable<InventoryEntry> GetPreconfiguredInventoryEntries()
        {
            return new List<InventoryEntry>
            {
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Lotus",
                    ExtranalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = Shared.Enums.Inventory.EDocumentType.Purchase,
                },
                new InventoryEntry(){
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Cadilac",
                    ExtranalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = Shared.Enums.Inventory.EDocumentType.Purchase,
                }
            };
        }
    }
}
