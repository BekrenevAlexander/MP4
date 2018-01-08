using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Thread recycleThread;

        private List<Tuple<Guid, Game>> games;

        private RoomsService()
        {
            rooms=new List<Room>();
            games=new List<Tuple<Guid, Game>>();
            recycleThread=new Thread(CheckActivity);
            recycleThread.Start();
        }
        public static RoomsService GetInstance()
        {
                if (_service == null)
                {
                    _service=new RoomsService();
                }
                return _service;
        }

        public void CheckActivity()
        {
            while (true)
            {
                try
                {

                    lock (games)
                    {
                        List<Tuple<Guid, Game>> endedGames =
                            games.Where(t => t.Item2.LastActionTime.Subtract(DateTime.Now).TotalMinutes < 1).ToList();
                        foreach (Tuple<Guid, Game> endedGame in endedGames)
                        {
                            games.Remove(endedGame);
                            lock (rooms)
                            {
                                rooms.RemoveAll(t => t.Id.Equals(endedGame.Item1));
                            }
                        }
                    }

                }
                catch (Exception e)
                {

                }
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }

        }

        public List<Room> GetRooms()
        {
            return rooms;
        }

        public Room GetRooms(Guid id)
        {
            return rooms.Find(t=>t.Id.Equals(id));
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
            Game game = new Game(room.FirstPlayerId,room.SecondPlayerId);
            Tuple<Guid,Game> tuple = new Tuple<Guid, Game>(room.Id,game);
            games.Add(tuple);
            return tuple;
        }

        public void RemoveGame(Guid id)
        {
            games.RemoveAll(t => t.Item1.Equals(id));
        }

        public Game GetGame(Guid id)
        {
            try
            {
                return games.Find(t => t.Item1.Equals(id)).Item2;
            }
            catch
            {
                return null;
            }
        }

        public Room GetRoom(Guid id)
        {
           return rooms.Find(t => t.Id.Equals(id));
        }
    }
}