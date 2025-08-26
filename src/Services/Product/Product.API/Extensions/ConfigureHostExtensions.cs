namespace Product.API.Extensions
{
    public static class ConfigureHostExtensions
    {
        public static void AddAppConfiguration(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var evn = context.HostingEnvironment;
                config.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            });
        }
    }
}
