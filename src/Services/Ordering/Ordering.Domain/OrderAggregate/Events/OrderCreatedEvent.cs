using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Common.Events;

namespace Ordering.Domain.OrderAggregate.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public string UserName { get; private set; }
        public string DocumentNo { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string ShippingAddress { get; private set; }
        public string InvoiceAddress { get; private set; }

        [NotMapped]
        public string Fullname { get; set; } 
        public OrderCreatedEvent(string userName, string documentNo, decimal totalPrice, string firstName, string lastName, string emailAddress, string shippingAddress, string invoiceAddress, string fullname)
        {
            UserName = userName;
            DocumentNo = documentNo;
            TotalPrice = totalPrice;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            ShippingAddress = shippingAddress;
            InvoiceAddress = invoiceAddress;
            Fullname = fullname;
        }
    }
}
