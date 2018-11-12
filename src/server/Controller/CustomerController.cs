using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hostel.Server.Models;
using Hostel.Server.Services;
using Newtonsoft.Json;

namespace Hostel.Server.Controllers
{

    [Route("customer/")]
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
                result = db.Customers.Where(c => c.Id == id).FirstOrDefault();
            }
            return result;
        }

        private bool IsNotUnique(Customer customer)
        {
            return this.DbService.Context.Customers.Any(c =>
                (c.FirstName == customer.FirstName)
                && (c.SecondName == customer.SecondName)
                && (c.ThirdName == customer.ThirdName));
        }

        [HttpPost("add/")]
        [HttpPost("")]
        public IActionResult RequestAdd()
        {
#if DEBUG
            Console.WriteLine("request:customer/add/");
#endif

            Customer customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Request.Body.Stringify());

            if (customer == null)
            {
                return new BadRequestResult();
            }

            if (this.IsNotUnique(customer))
            {
                return new ConflictResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            return new JsonResult(customer);
        }

        [HttpPost("remove/{id:int}")]
        [HttpDelete("{id:int}")]
        public IActionResult RequestRemove(int id)
        {
#if DEBUG
            Console.WriteLine("request:customer/remove/{id:int}");
#endif

            var customer = this.GetById(id);

            if (customer == null)
            {
                return new NotFoundResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            return new OkResult();
        }

        [HttpPost("get/{id:int}")]
        [HttpGet("{id:int}")]
        public IActionResult RequestGet(int id)
        {
#if DEBUG
            Console.WriteLine("request:customer/get/{id:int}");
#endif

            Customer result = this.GetById(id);

            if (result == null)
            {
                return new NotFoundResult();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPost("update/{id:int}")]
        [HttpPatch("{id:int}")]
        public IActionResult RequestUpdate(int id)
        {
#if DEBUG
            Console.WriteLine("request:customer/update/{id:int}");
#endif
            Customer newData = JsonConvert.DeserializeObject<Customer>(HttpContext.Request.Body.Stringify());

            if (newData == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new EmptyResult();
            }

            newData.Id = id;
            // TODO: set InhabitantId if none present;

            using (var db = this.DbService.Context)
            {
                db.Customers.Update(newData);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(this.GetById(id));
        }

        [HttpPost("all/")]
        [HttpGet("")]
        public IActionResult RequestGetAll()
        {
            List<Customer> result = this.DbService.Context.Customers.ToList();
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}