using MySqlConnector;
using Product.API.Presistance;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Product.API.Repositories.Interfaces;
using Product.API.Repositories;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfracstructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureProductDbContext(configuration);
            services.AddInfrastructureServices();

            return services;
        }

        private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var builder = new MySqlConnectionStringBuilder(connectionString);

            services.AddDbContext<ProductContext>(m => m.UseMySql(builder.ConnectionString, ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBase<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<IProductRepository, ProductRepository>()
                ;
        }
    }
}
