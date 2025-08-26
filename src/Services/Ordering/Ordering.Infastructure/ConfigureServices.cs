using Contracts.Common.Interfaces;
using Contracts.Messages;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Messages;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infastructure.Persistance;
using Ordering.Infastructure.Repositories;
using Ordering.Infastructure.Services;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfracstructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                    builder => builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
            });

            

            services.AddScoped<OrderContextSeed>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(ISmtpEmailServices), typeof(SmtpEmailService));

            services.AddScoped<IMessageProducer, RabbitMQProducer>();
            services.AddScoped<ISerializeServices, SerializeService>();
            return services;
        }
    }
}
