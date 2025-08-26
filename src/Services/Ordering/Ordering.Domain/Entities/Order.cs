using Contracts.Common.Events;
using Contracts.Domains;
using Ordering.Domain.Enums;
using Ordering.Domain.OrderAggregate.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order : AudiableEventEntity<long>
    {
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string UserName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        public Guid DocumentNo { get; set; } = new Guid();
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string EmailAddress { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string ShippingAddress { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string InvoiceAddress { get; set; }
        [NotMapped]
        public string Fullname => FirstName + " " + LastName;
        public EOrderStatus Status { get; set; }
        
        public Order AddedOrder()
        {
            AddDomainEvent(new OrderCreatedEvent(UserName, DocumentNo.ToString(), TotalPrice, FirstName, LastName, EmailAddress, ShippingAddress, InvoiceAddress, Fullname));
            return this;
        }

        public Order DeletedOrder()
        {
            AddDomainEvent(new OrderDeletedEvent(id));
            return this;
        }
    }
}
