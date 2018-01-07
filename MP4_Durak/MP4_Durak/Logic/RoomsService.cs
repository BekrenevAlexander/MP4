using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MP4_Durak.Models;

namespace MP4_Durak.Logic
{
    public class RoomsService
    {
        private static RoomsService _service;
        private List<Room> rooms;

        private RoomsService()
        {
            
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
    }
}