using Common.Loggin;
using Serilog;

namespace Customer.API.Extensions
{
    public static class ConfigHostExtensions
    {
        public static void AddAppConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsetting.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            })
                .UseSerilog(Serilogger.Configure);
        }
    }
}
