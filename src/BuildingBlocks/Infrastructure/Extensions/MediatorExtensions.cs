using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> domainEvents, ILogger logger)
        {
            //var domainEntities = context.ChangeTracker.Entries<IEventEntity>().Select(x => x.Entity)
            //    .Where(x => x.GetDomainEvents().Any()).ToList();

            //var domainEvents = domainEntities.SelectMany(x => x.GetDomainEvents()).ToList();

            //foreach (var item in domainEntities)
            //{
            //    item.ClearDomainEvent();
            //}

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
                var data = new SerializeService().Serialize(domainEvent);
                logger.Information($"\n-----\nA domain event has been published!\n" + $"Event: {domainEvent.GetType().Name}\n" + $"Data: {data})\n-----\n" );
            }
        }
    }
}
