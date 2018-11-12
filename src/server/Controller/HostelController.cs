using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Hostel.Server.Models;
using Hostel.Server.Services;


namespace Hostel.Server.Controllers
{

    [Route("hostel/")]
    [ApiController]
    public class HostelController : ControllerBase
    {

        private DatabaseService dbService { get; }

        private IConfiguration Config;

        public HostelController(DatabaseService service, ConfigurationService config)
        {
            this.dbService = service;
            this.Config = config.Config;
        }

        private Inhabitant GetById(int id)
        {
            Inhabitant result;
            using (var db = this.dbService.Context)
            {
                result = db.Inhabitants.Where(i => i.Id == id).FirstOrDefault();
            }
            return result;
        }

        private Inhabitant GetByRoomId(int id)
        {
            Inhabitant result;
            using (var db = this.dbService.Context)
            {
                result = db.Inhabitants.Where(i => i.Room.Id == id).FirstOrDefault();
            }
            return result;
        }

        private Inhabitant GetByCustomerId(int id)
        {
            Inhabitant result;
            using (var db = this.dbService.Context)
            {
                result = db.Inhabitants.Where(i => i.Customer.Id == id).FirstOrDefault();
            }
            return result;
        }

        private bool IsRoomFree(int roomId)
        {
            return this.GetByRoomId(roomId) == null;
        }

        private bool IsCustomerNotInhabited(int customerId)
        {
            return this.GetByCustomerId(customerId) == null;
        }

        private bool CanBeInhabited(int roomId, int customerId)
        {
            return this.IsRoomFree(roomId) && this.IsCustomerNotInhabited(customerId);
        }

        private object PrepareToJson(Inhabitant i)
        {
            if (i == null)
            {
                return null;
            }
            return new
            {
                Id = i.Id,
                RoomId = i.RoomId,
                CustomerId = i.CustomerId,
                CheckIn = i.CheckIn
            };
        }

        [HttpPost("checkin/{roomId:int}/{customerId:int}")]
        [HttpPost("{roomId:int}/{customerId:int}")]
        public IActionResult RequestChechIn(int roomId, int customerId)
        {

            if (!this.CanBeInhabited(roomId, customerId))
            {
                return new ConflictResult();
            }

            Inhabitant inhabitant;
            using (var db = this.dbService.Context)
            {
                inhabitant = new Inhabitant
                {
                    CustomerId = customerId,
                    RoomId = roomId,
                    CheckIn = DateTime.Now
                };
                db.Inhabitants.Add(inhabitant);
                db.SaveChanges();
            }

            using (var db = this.dbService.Context)
            {
                var customer = db.Customers.Where(c => c.Id == customerId).FirstOrDefault();
                customer.InhabitantId = inhabitant.Id;
                db.Customers.Update(customer);

                var room = db.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
                room.InhabitantId = inhabitant.Id;
                db.Rooms.Update(room);

                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            return new JsonResult(this.PrepareToJson(this.GetById(inhabitant.Id)));
        }

        [HttpPost("checkout/{id:int}")]
        [HttpDelete("{id:int}")]
        public IActionResult RequestChechOut(int id)
        {
            Inhabitant inhabitant = this.GetById(id);

            if (inhabitant == null)
            {
                return new NotFoundResult();
            }

            TimeSpan usingTime = DateTime.Now - inhabitant.CheckIn;

            Room room = this.dbService.Context.Rooms
                .Where(r => r.Id == inhabitant.RoomId).FirstOrDefault();
            RoomCategory category = room.Category;

            using (var db = this.dbService.Context)
            {
                db.Inhabitants.Remove(inhabitant);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new
            {
                Price = Logic.CalculatePrice(this.Config, category, usingTime)
            });
        }

        [HttpPost("get/{id:int}")]
        [HttpGet("{id:int}")]
        public IActionResult RequestGet(int id)
        {
            Inhabitant inhabitant = this.GetById(id);

            if (inhabitant == null)
            {
                return new NotFoundResult();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(this.PrepareToJson(inhabitant));
        }


        [HttpPost("get/")]
        [HttpGet("")]
        public IActionResult RequestGetAll()
        {
            var result = this.dbService.Context.Inhabitants
                .Select(this.PrepareToJson).ToList();

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}