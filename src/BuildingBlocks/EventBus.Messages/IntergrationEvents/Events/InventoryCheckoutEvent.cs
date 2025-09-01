using EventBus.Messages.IntergrationEvents.IntegrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.IntergrationEvents.Events
{
    public record InventoryCheckoutEvent() : IntergrationBaseEvent
    {
        public string Username { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }
    public class CartItem
    {
        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage = "The field {0} must be > = {1}.")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} must be > = {1}.")]
        public decimal ItemPrice { get; set; }
        [Required]
        public string ItemNo { get; set; }
        [Required]
        public string ItemName { get; set; }

        public double AvailableQuantity { get; set; }

    }
}
