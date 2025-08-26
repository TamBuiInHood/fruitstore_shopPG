using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Product.API.Extensions
{
    public static class HostExtensions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger =  services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating mysql database");
                    ExecuteMigration(context);
                    logger.LogInformation("Migrated mysql database");
                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception ex)
                {
                    logger.LogError("An Error occerred while migrate database to MySQL");
                }
            }
            return host;
        }
        private static void ExecuteMigration<TContext>(TContext context)
        where TContext : DbContext
        {
            context.Database.Migrate();
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}
