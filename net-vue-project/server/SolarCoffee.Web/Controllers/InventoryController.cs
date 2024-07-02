using Microsoft.AspNetCore.Mvc;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        public readonly IInventoryService _inventoryService;

        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public ActionResult GetInventories()
        {
            _logger.LogInformation("Getting inevntories...");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(inv => new ProductInventoryModel
                {
                    Id = inv.Id,
                    Product = ProductMapper.SerializeProductModel(inv.Product),
                    IdealQuantity = inv.IdealQuantity,
                    QuantityOnHand = inv.QuantityOnHand
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();
            return Ok(inventory); 
        }

        [HttpPatch]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            _logger.LogInformation($"Updating inventory {shipment.ProductId} ...");
            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }
    }
}
