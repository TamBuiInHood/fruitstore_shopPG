using AutoMapper;
using Infrastructure.Mapping;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.SeedWork;
namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public long Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.Status, opts => opts.Ignore())
                .IgnoreAllNonExisting();
                
        }
    }
}
