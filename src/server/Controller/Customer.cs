using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hostel.Server.Models;
using Hostel.Server.Services;
using Newtonsoft.Json;

namespace Hostel.Server.Controller
{

    [Route("customer/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private DatabaseService DbService { get; }

        public CustomerController(DatabaseService service)
        {
            this.DbService = service;
        }

        private Customer GetById(int id)
        {
            Customer result;
            using (var db = this.DbService.Context)
            {
                result = this.DbService.Context.Customers.Where(c => c.Id == id).First();
            }
            return result;
        }

        private Customer GetByCustomer(Customer customer)
        {
            Customer result;
            using (var db = this.DbService.Context)
            {
                result = this.DbService.Context.Customers.Where(c =>
                    c.FirstName == customer.FirstName
                    && c.SecondName == customer.SecondName
                    && c.ThirdName == customer.ThirdName).First();
            }
            return result;
        }

        private bool IsCustomerNull(Customer customer)
        {
            return (customer.FirstName == null)
                & (customer.SecondName == null)
                & (customer.ThirdName == null);
        }

        [HttpGet("add/")]
        [HttpPost("add/")]
        public IActionResult RequestAdd()
        {
            Customer customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Request.Body.Stringify());

            using (var db = this.DbService.Context)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(customer);
        }

        [HttpGet("remove/{id:int}")]
        [HttpPost("remove/{id:int}")]
        public IActionResult RequestRemove(int id)
        {

            var customer = this.GetById(id);

            if (customer == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new EmptyResult();
        }

        [HttpGet("get/{id:int}")]
        [HttpPost("get/{id:int}")]
        public IActionResult RequestGet(int id)
        {
            Customer result = this.GetById(id);

            if (result == null)
            {
                // TODO: make seperate class/method with 404 response;
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpGet("find/")]
        [HttpPost("find/")]
        public IActionResult RequestFind()
        {
            Customer customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Request.Body.Stringify());

            if (this.IsCustomerNull(customer))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new EmptyResult();
            }

            Customer result = this.GetByCustomer(customer);

            // TODO: submethod - return customer?;
            if (result == null)
            {
                // TODO: make seperate class/method with 404 response;
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}