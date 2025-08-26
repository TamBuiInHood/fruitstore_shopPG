using Shared.DTOs.CustomerDTO;

namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUsernameAsync(string username);
        Task<IResult> GetCustomersAsync();

        Task<IResult> UpdateCustomer(string username,UpdateCustomerDto updateCustomerDto);
        Task<IResult> CreateCustomer(CreateCustomerDto createCustomerDto);
        Task<IResult> DeleteCustomer(string username);
    }
}
