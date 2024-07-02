using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class OrderMapper
    {
        public static SalesOrder SerializeInvoiceOrder(InvoiceModel invoice)
        {
            var salesOrderItems = invoice.LineItems
                .Select(x => new SalesOrderItem
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    Product = ProductMapper.SerializeProductModel(x.Product)
                })
                .ToList();

            return new SalesOrder
            {
                SalesOrderItems = salesOrderItems,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                Id = invoice.Id
            };
        }

        public static List<OrderModel> SerializeOrdersToViewModels(IEnumerable<SalesOrder> orders)
        {
            return orders.Select(ord => new OrderModel 
            {
                Id = ord.Id,
                CreatedOn = ord.CreatedOn,
                UpdatedOn = ord.UpdatedOn,
                SalesOrderItems = SerializeSalesOrderItems(ord.SalesOrderItems),
                Customer = CustomerMapper.SerializeCustomer(ord.Customer),
                IsPaid = ord.IsPaid
            }).ToList();
        }

        private static List<SalesOrderItemModel> SerializeSalesOrderItems(List<SalesOrderItem> salesOrderItems)
        {
            return salesOrderItems.Select(s => new SalesOrderItemModel
            {
                Id = s.Id,
                Quantity = s.Quantity,
                Product = ProductMapper.SerializeProductModel(s.Product)
            }).ToList();
        }
    }
}
