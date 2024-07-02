using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDBContext _db;

        public ProductService(SolarDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Archives the product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);

                product.IsArchived = true;

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    data = product,
                    Time = DateTime.Now,
                    Message = "Product archived successfully",
                    Success = true
                };
            } catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    data = null,
                    Time = DateTime.Now,
                    Message = ex.Message,
                    Success = true
                };
            }
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Id = product.Id + 1,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };

                _db.ProductInventories.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product> {
                    data = product,
                    Time = DateTime.Now,
                    Message = "Message saved successfully",
                    Success = true
                };
            } catch(Exception e) 
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    data = product,
                    Time = DateTime.Now,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Return all available products
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Returns a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }
    }
}
