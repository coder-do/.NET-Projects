using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDBContext _db;
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDBContext db, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _db = db;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new order");

            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
                 item.Quantity = item.Quantity;
                var t = _inventoryService.GetCurrentInventory().Find(e => e.Product.Id == item.Product.Id);
                int inventoryId = _inventoryService.GetProductById(item.Product.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();

                return new ServiceResponse<bool> { 
                    data = true,
                    Time = DateTime.UtcNow,
                    Message = "Sale order added",
                    Success = true
                };
            } catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    data = false,
                    Time = DateTime.UtcNow,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(s => s.Customer)
                    .ThenInclude(s => s.PrimaryAddress)
                .Include(s => s.SalesOrderItems)
                    .ThenInclude(s => s.Product)
                .ToList();
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var order = _db.SalesOrders.Find(id);
            order.UpdatedOn = DateTime.UtcNow;  
            order.IsPaid = true;

            try
            {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    data = true,
                    Time = DateTime.UtcNow,
                    Message = $"Sale order with id {id} closed.",
                    Success = true
                };
            } catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    data = false,
                    Time = DateTime.UtcNow,
                    Message = e.Message,
                    Success = false
                };
            }
        }
    }
}
