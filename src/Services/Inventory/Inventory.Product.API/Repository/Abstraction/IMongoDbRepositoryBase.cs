using Inventory.Product.API.Entities.Abstraction;
using MongoDB.Driver;
using Shared.SeedWork;

namespace Inventory.Product.API.Repository.Abstraction
{
    public interface IMongoDbRepositoryBase<T> where T: MongoEntity 
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreferance = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<PageList<T>> PaginatedListAsync(FilterDefinition<T> filter, int pageIndex, int pageSize);

    }
}
