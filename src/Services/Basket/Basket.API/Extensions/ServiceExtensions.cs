using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);

            var cacheSettings = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
            services.AddSingleton(cacheSettings);

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services) => services.AddScoped<IBasketRepository, BasketRepository>()
            .AddTransient<ISerializeServices, SerializeService>();

        public static void ConfigRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var setting = services.GetOptions<CacheSettings>("CacheSettings");
            if (String.IsNullOrEmpty(setting.ConnectionStrings))
                throw new ArgumentNullException("Redis Connection string is not configured.");

            //Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                // truyen connection string cua docker vao
                options.Configuration = setting.ConnectionStrings;
            });
        }

        public static void ConfigMassTransit(this IServiceCollection services)
        {
            var setting = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (String.IsNullOrEmpty(setting.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured.");

            var mqConnection = new Uri(setting.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                // publish submit order message
                config.AddRequestClient<IBasketCheckoutEvent>();
            });
        }
    }
}
