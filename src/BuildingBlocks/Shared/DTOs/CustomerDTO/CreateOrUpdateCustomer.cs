using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CustomerDTO
{
    public abstract class CreateOrUpdateCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
