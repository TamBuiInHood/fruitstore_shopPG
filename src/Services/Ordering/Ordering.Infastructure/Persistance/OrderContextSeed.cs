using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infastructure.Persistance
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if(_context.Database.IsSqlServer())
                    await _context.Database.MigrateAsync();
            } catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initiallising the database");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (!_context.Orders.Any())
            {
                await _context.AddRangeAsync(new Order
                {
                    CreateDate = DateTime.Now,
                    EmailAddress = "thetamtbd@gmail.com",
                    FirstName = "customer1",
                    LastName = "customer1",
                    InvoiceAddress = "AUS",
                    LastModifiedDate = DateTime.Now,
                    ShippingAddress = "ABC HCM",
                    TotalPrice = 200,
                    UserName = "customer1"
                },
                new Order
                {
                    CreateDate = DateTime.Now,
                    EmailAddress = "thetamtbd@gmail.com",
                    FirstName = "customer2",
                    LastName = "customer2",
                    InvoiceAddress = "VN",
                    LastModifiedDate = DateTime.Now,
                    ShippingAddress = "ABC HCM",
                    TotalPrice = 150,
                    UserName = "customer2"
                });
            }
        }
    }
}
