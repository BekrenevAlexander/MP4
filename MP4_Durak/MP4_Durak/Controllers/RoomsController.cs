using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MP4_Durak.Logic;
using MP4_Durak.Models;

namespace MP4_Durak
{
    public class RoomsController : ApiController
    {
        // GET api/<controller>
        public List<Room> Get()
        {
            return RoomsService.GetInstance().GetRooms();
        }

        // POST api/<controller>
        public Room Post()
        {
            Room room = new Room(Guid.Parse(User.Identity.GetUserId()), User.Identity.Name, Guid.Parse(User.Identity.GetUserId()));
            RoomsService.GetInstance().AddRoom(room);
            return room;
        }
        // DELETE api/<controller>/5
        public void Delete()
        {
            RoomsService.GetInstance().RemoveRoom(Guid.Parse(User.Identity.GetUserId()));
        }
    }
}