using AutoMapper;
using EventBus.Messages.IntergrationEvents.Events;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;
using Inventory.Product.API.Services.Interfaces;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using RabbitMQ.Client;
using Shared.DTOs.InventoryDTO;
using static MassTransit.ValidationResultExtensions;

namespace Inventory.Product.API.Consumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly Serilog.ILogger _logger;
        private IInventoryServices _inventoryServices;

        public BasketCheckoutConsumer(Serilog.ILogger logger, IInventoryServices inventoryServices)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inventoryServices = inventoryServices ?? throw new ArgumentNullException(nameof(inventoryServices));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var purchaseDto = new PurchaseProductDto(item.ItemNo, -item.Quantity);

                var result = await _inventoryServices.PurchaseItemAsync(item.ItemNo, purchaseDto);

                _logger.Information("BasketCheckoutEvent consumed successfully." + "Purchase Inventory is created with Id: {newOrderId}", result.Id);
            }
        }
    }
}
