using AutoMapper;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Serilog;

namespace Ordering.API.Application.EventsHandler
{
    public class BasketCheckoutEventsHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public BasketCheckoutEventsHandler(IMediator mediator, IMapper mapper, Serilog.ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.Information("BasketCheckoutEvent consumed successfully." + "Order is created with Id: {newOrderId}", result.Data);
        }
    }
}
