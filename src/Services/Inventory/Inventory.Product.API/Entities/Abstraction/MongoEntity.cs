using Contracts.Domains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.Product.API.Entities.Abstraction
{
    public class MongoEntity 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; protected init; }

        [BsonElement("createDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [BsonElement("lastModifiedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    }
}
