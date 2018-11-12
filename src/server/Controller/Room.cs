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
                result = db.Rooms.Where(c => c.Number == id).FirstOrDefault();
            }
            return result;
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
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new EmptyResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
            }
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(room);
        }

        [HttpGet("remove/{id:int}")]
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
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            using (var db = this.DbService.Context)
            {
                db.Rooms.Remove(room);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new EmptyResult();
        }

        [HttpGet("{id:int}")]
        [HttpPost("get/{id:int}")]
        public IActionResult RequestGet(int id)
        {
#if DEBUG
            Console.WriteLine("request:room/get/{id:int}");
#endif

            Room result = this.GetById(id);

            if (result == null)
            {
                // TODO: make seperate class/method with 404 response;
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPost("update/{id}")]
        [HttpPatch("{id}")]
        public IActionResult RequestUpdate(int id)
        {
#if DEBUG
            Console.WriteLine("request:room/update/{id:int}");
#endif
            Room newData = JsonConvert.DeserializeObject<Room>(HttpContext.Request.Body.Stringify());

            if (newData == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new EmptyResult();
            }

            newData.Number = id;
            // TODO: set InhabitantId if none present;

            using (var db = this.DbService.Context)
            {
                db.Rooms.Update(newData);
                db.SaveChanges();
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(this.GetById(id));
        }

        [HttpGet("")]
        public IActionResult RequestGetAll()
        {
            List<Room> result = this.DbService.Context.Rooms.ToList();
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}