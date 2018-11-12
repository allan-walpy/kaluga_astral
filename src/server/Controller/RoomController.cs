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

    [Route("room/")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private DatabaseService DbService { get; }

        public RoomController(DatabaseService service)
        {
            this.DbService = service;
        }

        private Room GetById(int id)
        {
            Room result;
            using (var db = this.DbService.Context)
            {
                result = db.Rooms.Where(c => c.Id == id).FirstOrDefault();
            }
            return result;
        }

        private bool IsNotUnique(Room room)
        {
            return this.DbService.Context.Rooms.Any(r => r.Number == room.Number);
        }

        [HttpPost("add/")]
        [HttpPost("")]
        public IActionResult RequestAdd()
        {
#if DEBUG
            Console.WriteLine("request:room/add/");
#endif

            Room room = JsonConvert.DeserializeObject<Room>(HttpContext.Request.Body.Stringify());
            if (room == null)
            {
                return new BadRequestResult();
            }
            if (this.IsNotUnique(room))
            {
                return new ConflictResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            return new JsonResult(room);
        }

        [HttpPost("remove/{id:int}")]
        [HttpDelete("{id:int}")]
        public IActionResult RequestRemove(int id)
        {
#if DEBUG
            Console.WriteLine("request:room/remove/{id:int}");
#endif

            Room room = this.GetById(id);

            if (room == null)
            {
                return new NotFoundResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Rooms.Remove(room);
                db.SaveChanges();
            }

            return new OkResult();
        }

        [HttpPost("get/{id:int}")]
        [HttpGet("{id:int}")]
        public IActionResult RequestGet(int id)
        {
#if DEBUG
            Console.WriteLine("request:room/get/{id:int}");
#endif

            Room result = this.GetById(id);
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
            Console.WriteLine("request:room/update/{id:int}");
#endif
            Room newData = JsonConvert.DeserializeObject<Room>(HttpContext.Request.Body.Stringify());

            if (newData == null)
            {
                return new BadRequestResult();
            }

            newData.Id = id;

            using (var db = this.DbService.Context)
            {
                db.Rooms.Update(newData);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(this.GetById(id));
        }

        [HttpPost("all/")]
        [HttpGet("")]
        public IActionResult RequestGetAll()
        {
            List<Room> result = this.DbService.Context.Rooms.ToList();

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}