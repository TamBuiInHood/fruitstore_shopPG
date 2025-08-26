using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
