using AutoMapper;
using EventBus.Messages.IntergrationEvents.IntegrationEvents.Events;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<ApiResult<long>>, IMapFrom<Order>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [Range(0.1, double.PositiveInfinity,ErrorMessage = "Price must be >= 0.1")]
        public decimal TotalPrice { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string InvoiceAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderCommand, Order>();
            profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
        }
    }
}
