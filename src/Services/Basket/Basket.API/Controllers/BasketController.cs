using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this._repository = basketRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{username}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string username)
        {
            var result = await _repository.GetBaskeyByUserName(username);
            return Ok(result ?? new Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var result = await _repository.UpdateBasket(cart, options);
            return Ok(result);
        }


        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
        {
            var result = await _repository.DeleteBaskeyFromUserName(username);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBaskeyByUserName(basketCheckout.UserName);
            if (basket == null) return NotFound();

            // publish checkout event to eventbus Message
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            
            _publishEndpoint.Publish(eventMessage);

            //remove the basket
            await _repository.DeleteBaskeyFromUserName(basketCheckout.UserName);
            
            return Accepted();
        }
    }
}
