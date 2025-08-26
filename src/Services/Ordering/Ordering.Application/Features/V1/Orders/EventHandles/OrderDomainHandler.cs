using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.OrderAggregate.Events;
using Serilog;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.EventHandles
{
    public class OrderDomainHandler : INotificationHandler<OrderCreatedEvent>,
        INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger _logger;
        private readonly ISmtpEmailServices _smtpEmailServices;
        public OrderDomainHandler(ILogger logger, ISmtpEmailServices smtpEmailServices)
        {
            _logger = logger;
            _smtpEmailServices = smtpEmailServices;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Ordering domain event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Ordering domain event: {DomainEvent}", notification.GetType().Name);

            var emailRequest = new MailRequest
            {
                ToAddress = notification.EmailAddress,
                Body = $"Your Order detail." +
                $"<p> Order Id: {notification.DocumentNo}</p>" +
                $"<p> Total: {notification.TotalPrice}</p>",
                Subject = $"Hello {notification.Fullname}, your order was created"
            };
            try
            {
                _smtpEmailServices.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent created order email {notification.EmailAddress}");
            } catch (Exception ex)
            {
                _logger.Error($"Order {notification.EmailAddress} failed due to an error with email service: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
