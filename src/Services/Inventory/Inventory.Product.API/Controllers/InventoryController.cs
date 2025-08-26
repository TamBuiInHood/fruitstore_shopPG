using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.InventoryDTO;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryServices _inventoryServices;

        public InventoryController(IInventoryServices inventoryServices)
        {
            _inventoryServices = inventoryServices;
        }

        // api/inventory/items/{itemno}
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _inventoryServices.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }


        // api/inventory/items/{itemno}/paging
        [Route("items/{itemNo}/paging", Name = "GetAllByItemNo")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo, [FromQuery]GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryServices.GetAllbyItemNoPagin(query);
            return Ok(result);
        }
    }
}
