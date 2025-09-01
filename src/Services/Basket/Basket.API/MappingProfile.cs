using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.IntergrationEvents.Events;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;

namespace Basket.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
            CreateMap<Basket.API.Entities.CartItem,
                  EventBus.Messages.IntergrationEvents.IntegrationEvents.Events.CartItem>()
            .ReverseMap();
        }
    }
}
