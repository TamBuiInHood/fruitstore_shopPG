using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class RepositoryBase<T, K, TContext> : RepositoryQueryBase<T, K, TContext>,
        IRepositoryBaseAsync<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {

        private readonly TContext _dbContext;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBase(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Create(T entity)
        => _dbContext.Set<T>().Add(entity);

        public async Task<K> CreateSaveAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity.id;
        }

        public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities.Select(x => x.id).ToList();
        }

        public async Task<IList<K>> CreateListSaveAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities.Select(x => x.id).ToList();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _dbContext.Database.BeginTransactionAsync();
        }

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.CommitAsync();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteListSaveAsync(IEnumerable<T> entites)
        {
            _dbContext.Set<T>().RemoveRange(entites);
            await SaveChangesAsync();
            return ;
        }

        public Task DeleteListAsync(IEnumerable<T> entites)
        {
            _dbContext.Set<T>().RemoveRange(entites);
            return Task.CompletedTask;
        }

        public async Task DeleteSaveAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveChangesAsync();
            return;
        }

        public void Update(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged)
                return;
            T exist = _dbContext.Set<T>().Find(entity.id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        }

        public async Task UpdateSaveAsync(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged)
                return;
            T exist = _dbContext.Set<T>().Find(entity.id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            await SaveChangesAsync();
        }

        public Task UpdateListAsync(IEnumerable<T> entites)
        {
            return _dbContext.Set<T>().AddRangeAsync(entites);
        }

        public async Task UpdateListSaveAsync(IEnumerable<T> entites)
        {
            _dbContext.Set<T>().AddRangeAsync(entites);
            await SaveChangesAsync();
        }

    }
}
