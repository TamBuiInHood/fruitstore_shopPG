using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public abstract class CreateOrUpdateProductDto
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Maximum length for Prduct Name is 250 Chars.")]
        public string Name { get; set; }
        
        [MaxLength(250, ErrorMessage = "Maximum length for Prduct Name is 250 Chars.")]
        public string Summary { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
