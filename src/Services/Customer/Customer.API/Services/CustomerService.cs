using AutoMapper;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CustomerDTO;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var checkExistUserName = await _customerRepository.GetCustomerByUsernameAsync(createCustomerDto.Username);
            if (checkExistUserName != null)
            {
                return Results.BadRequest(new { error = "This username has exist" });
            }
            var checkExistUserEmail = await _customerRepository.FindByCondition(x => x.EmailAddress.Equals(createCustomerDto.EmailAddress)).AnyAsync();
            if (checkExistUserEmail)
            {
                return Results.BadRequest(new { error = "This username has exist" });
            }

            var newCustomer = _mapper.Map<Entities.Customer>(createCustomerDto);
            await _customerRepository.CreateSaveAsync(newCustomer);
            await _customerRepository.SaveChangesAsync();
            var result = _mapper.Map<CustomerDto>(newCustomer);
            return Results.Ok(result);
        }

        public async Task<IResult> DeleteCustomer(string username)
        {
            var checkExistCustomer = await _customerRepository.GetCustomerByUsernameAsync(username);
            if (checkExistCustomer == null)
            {
                return Results.NotFound(new { error = "This username has non-exist" });
            }
            await _customerRepository.DeleteSaveAsync(checkExistCustomer!);
            await _customerRepository.SaveChangesAsync();
            var result = _mapper.Map<CustomerDto>(checkExistCustomer);
            return Results.Ok(result);
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        => Results.Ok(await _customerRepository.GetCustomerByUsernameAsync(username));

        public async Task<IResult> GetCustomersAsync()
        => Results.Ok(await _customerRepository.GetCustomerAsync());

        public async Task<IResult> UpdateCustomer(string username, UpdateCustomerDto updateCustomerDto)
        {
            var checkExistCustomer = await _customerRepository.GetCustomerByUsernameAsync(username);
            if (checkExistCustomer == null)
            {
                return Results.NotFound(new { error = "This username has non-exist" });
            }

            var updatedCustomer = _mapper.Map(updateCustomerDto, checkExistCustomer);
            await _customerRepository.UpdateSaveAsync(checkExistCustomer!);
            await _customerRepository.SaveChangesAsync();
            var result = _mapper.Map<CustomerDto>(updatedCustomer);
            return Results.Ok(result);
        }
    }
}
