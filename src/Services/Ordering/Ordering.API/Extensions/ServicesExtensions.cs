using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServicesExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting)).Get<SMTPEmailSetting>();
            services.AddSingleton(emailSettings);

            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);
            return services;
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var setting = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (String.IsNullOrEmpty(setting.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured.");

            var mqConnection = new Uri(setting.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventsHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    //cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckOutConsumer>(ctx);
                    //});
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
