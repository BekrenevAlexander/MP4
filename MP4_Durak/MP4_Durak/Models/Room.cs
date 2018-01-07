using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MP4_Durak.Models
{
    public class Room
    {
        Guid id;
        string name;
        Guid firstPlayerId;
        Guid secondPlayerId;

        public Room(Guid id, string name, Guid firstPlayerId, Guid secondPlayerId)
        {
            this.id = id;
            this.name = name;
            this.firstPlayerId = firstPlayerId;
            this.secondPlayerId = secondPlayerId;
        }

        public Room(Guid id, string name, Guid firstPlayerId)
        {
            this.id = id;
            this.name = name;
            this.firstPlayerId = firstPlayerId;
        }

        public Room()
        {
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Guid FirstPlayerId
        {
            get { return firstPlayerId; }
            set { firstPlayerId = value; }
        }

        public Guid SecondPlayerId
        {
            get { return secondPlayerId; }
            set { secondPlayerId = value; }
        }
    }
}