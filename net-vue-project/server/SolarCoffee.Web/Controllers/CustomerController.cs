using Microsoft.AspNetCore.Mvc;
using SolarCoffee.Services.Customer;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            _logger.LogInformation("Creating customer");
            customer.CreatedOn = DateTime.UtcNow;
            customer.UpdatedOn = DateTime.UtcNow;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet]
        public ActionResult GetCustomers()
        {
            _logger.LogInformation("Getting customers");
            var customers = _customerService.GetAllCustomers();
            var customerModels = customers.Select(customer => new CustomerModel
            {
                Id = customer.Id,
                FirstName = customer.Name,
                LastName = customer.LastName,
                PrimaryAddress = new CustomerAddressModel
                {
                    Id = customer.Id,
                    Address1 = customer.PrimaryAddress.AddressLine1,
                    Address2 = customer.PrimaryAddress.AddressLine2,
                    City = customer.PrimaryAddress.City,
                    State = customer.PrimaryAddress.State,
                    Country = customer.PrimaryAddress.Country,
                    PostalCode = customer.PrimaryAddress.PostalCode,
                    CreatedOn = customer.PrimaryAddress.CreatedOn,
                    UpdatedOn = customer.PrimaryAddress.UpdatedOn,
                },
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            })
                .OrderByDescending(c => c.CreatedOn)
                .ToList();
            return Ok(customerModels);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation($"Deleting customer - {id}");
            var res = _customerService.DeleteCustomer(id);
            return Ok(res);
        }
    }
}
