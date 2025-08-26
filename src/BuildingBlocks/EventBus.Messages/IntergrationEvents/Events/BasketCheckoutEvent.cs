using EventBus.Messages.IntergrationEvents.IntegrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.IntergrationEvents.IntegrationEvents.Events
{
    public record BasketCheckoutEvent() : IntergrationBaseEvent, IBasketCheckoutEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }
    }
}
