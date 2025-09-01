using Common.Loggin;
using Serilog;

namespace Product.API.Extensions
{
    public static class ConfigureHostExtensions
    {
        public static void AddAppConfiguration(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var evn = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            });
            host.UseSerilog(Serilogger.Configure);
        }
    }
}
