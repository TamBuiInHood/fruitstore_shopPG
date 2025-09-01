using Inventory.Product.API.Entities;
using Inventory.Product.API.Entities.Abstraction;
using Inventory.Product.API.Extensions;
using MongoDB.Driver;
using Shared.SeedWork;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inventory.Product.API.Repository.Abstraction
{
    public class MongoRepository<T> : IMongoDbRepositoryBase<T>
    where T : MongoEntity
    {
        private IMongoDatabase database { get; }

        public MongoRepository(IMongoClient client, MongoDbSettings settings)
        {
            database = client.GetDatabase(settings.DatabaseName);
        }

        public Task CreateAsync(T entity)
        => Collection.InsertOneAsync(entity);

        public Task DeleteAsync(string id)
        => Collection.DeleteOneAsync(x => x.Id.Equals(id));

        public IMongoCollection<T> FindAll(ReadPreference? readPreferance = null)
            => database.WithReadPreference(readPreferance ?? ReadPreference.Primary)
            .GetCollection<T>(GetCollectionName());

        public async Task<PageList<T>> PaginatedListAsync( FilterDefinition<T> filter, int pageIndex, int pageSize)
        => await PageList<T>.ToPagedList(Collection,filter, pageIndex, pageSize);

        protected virtual IMongoCollection<T> Collection => database.GetCollection<T>(GetCollectionName());

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)entity.GetType()
                .GetProperty(func.Body.ToString()
                .Split(".")[1])?.GetValue(entity, null);

            var filter = Builders<T>.Filter.Eq(func, value);
            return Collection.ReplaceOneAsync(filter, entity);
        }

        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }
    }
}
