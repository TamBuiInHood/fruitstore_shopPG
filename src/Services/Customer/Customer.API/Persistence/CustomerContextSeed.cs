using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence
{
    public static class CustomerContextSeed
    {
        public static IHost SeedCustomerData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var customerContext = scope.ServiceProvider
                .GetService<CustomerContext>();
            customerContext.Database.MigrateAsync().GetAwaiter().GetResult();

            CreateCustomer(customerContext, "customer1", "customer1", "customer1@gmail.com", "customer1").GetAwaiter().GetResult();
            CreateCustomer(customerContext,  "customer2", "customer2", "customer2@gmail.com", "customer2").GetAwaiter().GetResult();
            
            return host;

        }

        private static async Task CreateCustomer(CustomerContext customerContext, string firstname, string lastName, string email, string username)
        {
            var customer = await customerContext.Customers
                .SingleOrDefaultAsync(x => x.UserName.Equals(username) ||
                x.EmailAddress.Equals(email));
            if(customer == null)
            {
                var newCustomer = new Entities.Customer
                {
                    UserName = username,
                    FirstName = firstname,
                    LastName = lastName,
                    EmailAddress = email
                };
                await customerContext.Customers.AddAsync(newCustomer);
                await customerContext.SaveChangesAsync();
            }
        }
    }
}
