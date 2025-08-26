using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Common.Interfaces
{
    public interface IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
    {
        IQueryable<T> FindAll(bool trackChanges = false);
        IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetByIdAsync(K Id);
        Task<T?> GetByIdAsync(K Id, params Expression<Func<T, object>>[] includeProperties);
    }

    // dung de Create-Update-Delete
    public interface IRepositoryBaseAsync<T, K> : IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
    {
        void Create(T entity);
        Task<K> CreateSaveAsync(T entity);
        Task<IList<K>> CreateListAsync(IEnumerable<T> entities);
        Task<IList<K>> CreateListSaveAsync(IEnumerable<T> entities);
        void Update(T entity);
        Task UpdateSaveAsync(T entity);
        Task UpdateListAsync(IEnumerable<T> entites);
        Task UpdateListSaveAsync(IEnumerable<T> entites);
        void Delete(T entity);
        Task DeleteSaveAsync(T entity);
        Task DeleteListAsync(IEnumerable<T> entites);
        Task DeleteListSaveAsync(IEnumerable<T> entites);
        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task EndTransactionAsync();
        Task RollbackTransactionAsync();
    }

    // giai quyet cac van de lay du lieu - GET
    public interface IRepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K> 
        where T : EntityBase<K>
        where TContext : DbContext
    {
        //IQueryable<T> FindAll(bool trackChanges = false);
        //IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
        //IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

        //IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

        //Task<T> GetByIdAsync(K Id);
        //Task<T?> GetByIdAsync(K Id, params Expression<Func<T, object>>[] includeProperties);
    }

    // dung de Create-Update-Delete
    public interface IRepositoryBaseAsync<T, K, TContext>  : IRepositoryBaseAsync<T, K>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        //Task<K> CreateAsync(T Entity);
        //Task<IList<K>> CreateListAsync(IEnumerable<T> entities);
        //Task UpdateAsync(T entity);
        //Task UpdateListAsync(IEnumerable<T> entites);
        //Task DeleteAsync(T entity);
        //Task DeleteListAsync(IEnumerable<T> entites);
        //Task<int> SaveChangesAsync();

        //Task<IDbContextTransaction> BeginTransactionAsync();
        //Task EndTransactionAsync();
        //Task RollbackTransactionAsync();
    }
}
