using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity;
using MP4_Durak.Logic;
using MP4_Durak.Models;

namespace MP4_Durak
{
    public class RoomsController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public List<Room> Get()
        {
            return RoomsService.GetInstance().GetRooms();
        }

        // POST api/<controller>
        [HttpPost]
        public Room Post()
        {
            Room room = new Room(Guid.Parse(User.Identity.GetUserId()), User.Identity.Name, Guid.Parse(User.Identity.GetUserId()));
            RoomsService.GetInstance().AddRoom(room);
            return room;
        }
        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete()
        {
            RoomsService.GetInstance().RemoveRoom(Guid.Parse(User.Identity.GetUserId()));
        }
        [HttpPost]
        [ActionName("connect")]
        public void Connect(Room room)
        {
            RoomsService.GetInstance().CreateGame(room);
        }
    }
}