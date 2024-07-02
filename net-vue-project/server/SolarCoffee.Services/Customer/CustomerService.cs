using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDBContext _db;

        public CustomerService(SolarDBContext db)
        {
            _db = db;
        }

        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Customer>
                {
                    data = customer,
                    Time = DateTime.Now,
                    Message = "New customer added",
                    Success = true
                };
            } catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    data = customer,
                    Time = DateTime.Now,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _db.Customers.FirstOrDefault(x => x.Id == id);

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    data = false,
                    Time = DateTime.Now,
                    Message = $"Customer with id {id} not found",
                    Success = false
                };
            } else
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    data = true,
                    Time = DateTime.Now,
                    Message = $"Customer with id {id} removed",
                    Success = true
                };
            }
        }

        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(c => c.PrimaryAddress)
                .OrderBy(c => c.LastName)
                .ToList();
        }

        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}
