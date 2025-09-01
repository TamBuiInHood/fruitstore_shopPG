using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryDTO
{
    public class InventoryEntryDto
    {
        public string Id { get; set; }
        public EDocumentType DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string itemNo { get; set; }
        public int Quantity { get; set; }
        public string ExtranalDocumentNo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
