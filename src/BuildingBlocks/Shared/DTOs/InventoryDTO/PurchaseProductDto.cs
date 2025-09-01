using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryDTO
{
    public class PurchaseProductDto
    {
        public EDocumentType DocumentType => EDocumentType.Purchase;
        public string ItemNo { get; private set; }
        public string DocumentNo { get; set; }
        public string ExtranalDocumentNo { get; set; } 
        public int Quantity { get; set; }

        public PurchaseProductDto(string itemNo, int quantity)
        {
            ItemNo = itemNo;
            Quantity = quantity;
        }
    }
}
