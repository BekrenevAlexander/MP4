using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Durak.Api;
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

        [HttpGet]
        [ActionName("getGame")]
        public Game GetGame(string roomId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(roomId));
        }

        // POST api/<controller>
        [HttpPost]
        public Room Post()
        {
            var context = ApplicationDbContext.Create();
            var usersContext = context.Users;
            ApplicationUser user = usersContext.Find(Guid.Parse(User.Identity.GetUserId()));
            Room room = new Room(Guid.Parse(User.Identity.GetUserId()), User.Identity.Name, Guid.Parse(User.Identity.GetUserId()),user.Games,user.Wins);
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
        public Game Connect(string roomId)
        {
            Room room=RoomsService.GetInstance().GetRoom(Guid.Parse(roomId));
            room.SecondPlayerId= Guid.Parse(User.Identity.GetUserId());
            return RoomsService.GetInstance().CreateGame(room).Item2;
        }
    }
}