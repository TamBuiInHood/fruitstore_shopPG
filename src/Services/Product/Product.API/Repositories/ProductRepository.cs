using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Presistance;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBase<CatalogProduct, long, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreateProduct(CatalogProduct product)
        => CreateSaveAsync(product);

        public async Task DeleteProduct(long id)
        {
            var product = await GetProduct(id);
            if (product != null) DeleteSaveAsync(product);
        }

        public Task<CatalogProduct> GetProduct(long id)
        => GetByIdAsync(id);

        public Task<CatalogProduct> GetProductByNo(string productNo)
        => FindByCondition(x => x.No.Equals(productNo)).FirstOrDefaultAsync();

        public async Task<IEnumerable<CatalogProduct>> GetProducts()
         => await FindAll().ToListAsync();

        public Task UpdateProduct(CatalogProduct product)
        => UpdateSaveAsync(product);
    }
}
