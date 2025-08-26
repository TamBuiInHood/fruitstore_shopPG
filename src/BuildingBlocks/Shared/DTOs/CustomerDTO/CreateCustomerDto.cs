using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CustomerDTO
{
    public class CreateCustomerDto : CreateOrUpdateCustomer
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}
