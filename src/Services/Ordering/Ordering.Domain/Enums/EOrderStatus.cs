using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum EOrderStatus
    {
        New = 1, // start with 1, 0 is used for filter all = 0
        Pending, // order is pending
        Paid, // order is paid
        Shipping, // Order is on the way 
        Fulfilled, // order is fulfilled -- > completed
    }
}
