using System;
using System.Collections.Generic;
using System.Linq;
using MP4_Durak.Models;

namespace Durak.Api
{
    public class Player
    {
        List<int> _cards;
        Guid id;
        Queue<string> messages; 


        public Player(List<int> cards,Guid id)
        {
            _cards = cards;
            this.id=id;
            messages=new Queue<string>();
        }

        public Guid Id
        {
            get { return id; }
        }

        public List<int> Cards
        {
            get
            {
                int[] buffList= new int[_cards.Count];
                _cards.CopyTo(buffList);
                return buffList.ToList();
            }
        }

        public void AddCards(List<int> newCards)
        {
            if (CardUtil.CheckForDuplicateCards(_cards, newCards))
            {
                throw new Exception("Найден дубликат карт");
            }
            _cards.AddRange(newCards);
        }

        public bool CheckExistCard(int card)
        {
           return _cards.Any(t => t == card);
        }

        public void RemoveCard(int card)
        {
            _cards.Remove(card);
        }

        public void AddMessage(string message)
        {
            lock (messages)
            {
                messages.Enqueue(message);
            }
        }

        public List<string> GetMessages()
        {
            List<string> buffList = new List<string>();
            lock (messages)
            {
                while (messages.Count>0)
                {
                    buffList.Add(messages.Dequeue());
                }
            }
            return buffList;
        } 
    }
}
