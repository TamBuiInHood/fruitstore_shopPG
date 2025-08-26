using Customer.API.Services.Interfaces;
using Shared.DTOs.CustomerDTO;

namespace Customer.API.Controller
{
    public static class CustomerController
    {
        public static void MapCustomerAPIs(this WebApplication app)
        {
            // GET
            app.MapGet("/api/customers", async (ICustomerService cusService) => await cusService.GetCustomersAsync());
            app.MapGet("/api/customers/{username}", async (string username, ICustomerService cusService) => await cusService.GetCustomerByUsernameAsync(username));
            // CREATE
            app.MapPost("/api/customers", async (CreateCustomerDto dto, ICustomerService service) =>
                    await service.CreateCustomer(dto));
            // UPDATE
            app.MapPut("/api/customers/{username}", async (string username, UpdateCustomerDto dto, ICustomerService service) =>
                await service.UpdateCustomer(username, dto)
            );
            // DELETE
            app.MapDelete("/api/customers/{username}", async (string username, ICustomerService service) =>
                await service.DeleteCustomer(username)
            );
        }
    }
}
