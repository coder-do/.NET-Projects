using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDBContext _db;
        private readonly ILogger _logger;

        public InventoryService(SolarDBContext db, ILogger<InventoryService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;

            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = now,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            _db.ProductInventorySnapshot.Add(snapshot);
            _db.SaveChanges();
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(p => p.Product)
                .Where(p => !p.Product.IsArchived)
                .ToList();
        }

        public ProductInventory GetProductById(int id)
        {
            return _db.ProductInventories
                .Include(p => p.Product)
                .FirstOrDefault(p => p.Product.Id == id);
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.Now - TimeSpan.FromHours(6);
            return _db.ProductInventorySnapshot
                .Include(p => p.Product)
                .Where(p => p.SnapshotTime > earliest
                    && !p.Product.IsArchived)
                .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                    .Include(p => p.Product)
                    .First(p => p.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                } catch (Exception ex)
                {
                    _logger.LogError($"Snapshot creation failed for product ${inventory.Product.Id}");
                    _logger.LogError(ex.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    data = inventory,
                    Time = DateTime.Now,
                    Message = $"Units for ${inventory.Product.Id} increased by ${adjustment}",
                    Success = true
                };
            } catch (Exception e)
            {
                return new ServiceResponse<ProductInventory>
                {
                    data = null,
                    Time = DateTime.Now,
                    Message = e.Message,
                    Success = false
                };
            }
        }
    }
}
