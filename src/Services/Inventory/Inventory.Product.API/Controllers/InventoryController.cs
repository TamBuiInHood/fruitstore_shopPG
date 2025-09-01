using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.InventoryDTO;
using Shared.SeedWork;
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
        [HttpGet]
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _inventoryServices.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        // api/inventory/{id}
        [HttpGet]
        [Route("{id}", Name = "GetAllById")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> GetAllById([Required] string id)
        {
            var result = await _inventoryServices.GetByIdAsync(id);
            return Ok(result);
        }


        // api/inventory/items/{itemno}/paging
        [HttpGet]
        [Route("items/{itemNo}/paging", Name = "GetAllByItemNoPagin")]
        [ProducesResponseType(typeof(PageList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PageList<InventoryEntryDto>>> GetAllByItemNoPagin([Required] string itemNo, [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryServices.GetAllbyItemNoPagin(query);
            return Ok(result);
        }

        // api/inventory/purchase/{itemNo}
        [HttpPost("purchase/{itemNo}", Name = "PurchaseOrder")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo, [FromBody] PurchaseProductDto model)
        {

            var result = await _inventoryServices.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }

        // api/inventory/{id}
        [HttpDelete("{id}", Name = "DeleteById")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> DeleteById([Required] string id)
        {
            var entity = await _inventoryServices.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await _inventoryServices.DeleteAsync(id);
            return NoContent();
        }

        // api/inventory/items/stock-vailable/{itemNo}
        [HttpGet("items/stock-vailable/{itemNo}", Name = "GetAvailableQuantity")]
        //[ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<double>> GetAvailableQuantity([Required] string itemNo)
        {
            var result = await _inventoryServices.GetAvailableQuantity(itemNo);
            return Ok(result);
        }
    }
}
