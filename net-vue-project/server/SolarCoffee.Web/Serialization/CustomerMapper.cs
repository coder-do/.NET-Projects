using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class CustomerMapper
    {
        public static CustomerModel SerializeCustomer(Customer customer)
        {
            var address = new CustomerAddressModel
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
            };

            return new CustomerModel
            {
               Id = customer.Id,
               CreatedOn = customer.CreatedOn, 
               UpdatedOn = customer.UpdatedOn,
               FirstName = customer.Name, 
               LastName = customer.LastName,
               PrimaryAddress = address,
            };
        }

        public static Customer SerializeCustomer(CustomerModel customer)
        {
            var address = new CustomerAddresses
            {
                Id = customer.Id,
                AddressLine1 = customer.PrimaryAddress.Address1,
                AddressLine2 = customer.PrimaryAddress.Address2,
                City = customer.PrimaryAddress.City,
                State = customer.PrimaryAddress.State,
                Country = customer.PrimaryAddress.Country,
                PostalCode = customer.PrimaryAddress.PostalCode,
                CreatedOn = customer.PrimaryAddress.CreatedOn,
                UpdatedOn = customer.PrimaryAddress.UpdatedOn,
            };

            return new Customer
            {
                Id = customer.Id,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                Name = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = address,
            };
        }
    }
}