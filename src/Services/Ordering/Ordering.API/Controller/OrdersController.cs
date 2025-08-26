using AutoMapper;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Features.V1.Orders.Commands;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Ordering.Application.Features.V1.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISmtpEmailServices _emailServices;
        private readonly IMessageProducer _messageProducer;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrdersController(IMediator mediator, ISmtpEmailServices smtpEmailServices, IMessageProducer messageProducer, IOrderRepository orderRepository, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _emailServices = smtpEmailServices;
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(mediator));
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByUserName([Required] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);

        }

        [HttpGet("send-email")]
        public async Task<IActionResult> SendEmail()
        {
            var message = new MailRequest()
            {
                Body = "<h1>Hello</h1>",
                Subject = "Test",
                ToAddress = "tambtse171869@fpt.edu.vn"
            };

            await _emailServices.SendEmailAsync(message);
            return Ok();
        }

        [HttpPost("{id:long}", Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(IEnumerable<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<long>> CreateOrder([FromBody] CreateOrderCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);
            return Ok(result);
        }

        [HttpPut("", Name = RouteNames.UpdateOrder)]
        [ProducesResponseType(typeof(IEnumerable<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<long>> UpdateOrder([FromBody] UpdateOrderCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);
            return Ok(result);
        }

        [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
        [ProducesResponseType(typeof(IEnumerable<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<long>> DeleteOrder( long id)
        {
            var deleteCommand = new DeleteOrderCommand(id);
            var result = await _mediator.Send(deleteCommand);
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        //{
        //    var order = _mapper.Map<Order>(orderDto);
        //    var addedOrder = await _orderRepository.CreateOrder(order);
        //    var result = _mapper.Map<OrderDto>(addedOrder);
        //    await _orderRepository.SaveChangesAsync();
        //    _messageProducer.SendMessage(result);
        //    return Ok(addedOrder);
        //}
    }
}
