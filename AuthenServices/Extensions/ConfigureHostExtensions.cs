using Common.Loggin;
using Serilog;

namespace AuthenServices.Extensions
{
    public static class ConfigureHostExtensions
    {
        public static void AddAppConfiguration(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                var evn = context.HostingEnvironment;
                config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            });
            host.UseSerilog(Serilogger.Configure);
        }
    }
}
