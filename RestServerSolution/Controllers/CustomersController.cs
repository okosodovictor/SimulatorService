using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestServerSolution.Models;
using RestServerSolution.Services;
using static RestServerSolution.Models.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestServerSolution.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
            {
                return BadRequest("No customers provided.");
            }

            var validationErrors = new List<string>();
            foreach (var customer in customers)
            {
                var exist = CustomerService.HasCustomer(customer.Id);
                if (exist)
                {
                    validationErrors.Add($"Customer With Id: {customer.Id} already exist.");
                }

                var validationResult = customer.Validate();
                if (validationResult != null && !validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                    var errorMessage = string.Join(", ", errors);
                    validationErrors.Add(errorMessage);
                }
            }

            if (validationErrors.Count > 0)
            {
                return BadRequest($"Validation failed: {string.Join(", ", validationErrors)}");
            }

            //Add the customers.
            CustomerService.AddCustomers(customers);

            return Ok("Customers added successfully");
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var allCustomers = CustomerService.GetCustomers();
            return Ok(allCustomers);
        }

    }
}

