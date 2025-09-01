using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AuthenDTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Must be email format!")]
        [DisplayName("Email Address")]
        public string Email { get; set; } = "";

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; } = "";
    }
}
