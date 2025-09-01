using Inventory.Product.API.Entities.Abstraction;
using Inventory.Product.API.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums.Inventory;

namespace Inventory.Product.API.Entities
{
    [BsonCollection("InventoryEntries")]
    public class InventoryEntry : MongoEntity
    {
        public InventoryEntry()
        {
            DocumentType = EDocumentType.Purchase;
            DocumentNo = Guid.NewGuid().ToString();
            ExtranalDocumentNo = Guid.NewGuid().ToString();
        }

        public InventoryEntry(string id) => (id) = id;

        [BsonElement("documentType")]
        public EDocumentType DocumentType { get; set; }
        [BsonElement("documentNo")]
        public string DocumentNo { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("itemNo")]
        public string ItemNo { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("extranalDocumentNo")]
        public string ExtranalDocumentNo { get; set; } = Guid.NewGuid().ToString();
    }
}
