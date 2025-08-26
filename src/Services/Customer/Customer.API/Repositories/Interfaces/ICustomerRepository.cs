using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryBaseAsync<Entities.Customer, int, CustomerContext>
    {
        Task<Entities.Customer> GetCustomerByUsernameAsync(string username);

        Task<IEnumerable<Entities.Customer>> GetCustomerAsync();
    }
}
