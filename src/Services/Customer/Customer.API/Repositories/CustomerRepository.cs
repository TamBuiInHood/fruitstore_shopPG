using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBase<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<Entities.Customer>> GetCustomerAsync()
        => await FindAll().ToListAsync();

        public Task<Entities.Customer> GetCustomerByUsernameAsync(string username)
        => FindByCondition(x => x.UserName.Equals(username)).FirstOrDefaultAsync()!;
    }
}
