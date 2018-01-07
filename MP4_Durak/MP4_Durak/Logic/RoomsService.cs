using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using Durak.Api;
using Microsoft.AspNet.Identity;
using MP4_Durak.Models;

namespace MP4_Durak.Logic
{
    public class RoomsService
    {
        private static RoomsService _service;
        private List<Room> rooms;

        private List<Tuple<Guid, Game>> games;

        private RoomsService()
        {
            rooms=new List<Room>();
            games=new List<Tuple<Guid, Game>>();
        }
        public static RoomsService GetInstance()
        {
            lock (_service)
            {
                if (_service == null)
                {
                    _service=new RoomsService();
                }
                return _service;
            }
        }

        public List<Room> GetRooms()
        {
            return rooms;
        }

        public void AddRoom(Room room)
        {
            rooms.Add(room);
        }

        public void RemoveRoom(Guid id)
        {
            rooms.RemoveAll(t => t.Id.Equals(id));
        }

        public Tuple<Guid, Game> CreateGame(Room room)
        {
            Game game = new Game();
            Tuple<Guid,Game> tuple = new Tuple<Guid, Game>(room.Id,game);
            games.Add(tuple);
            return tuple;
        }

        public void RemoveGame(Guid id)
        {
            games.RemoveAll(t => t.Item1.Equals(id));
        }
    }
}