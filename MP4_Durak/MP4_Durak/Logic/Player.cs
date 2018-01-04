using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Api
{
    class Player
    {
        List<int> _cards;
        string _name;

        public Player(List<int> cards, string name)
        {
            _cards = cards;
            _name = name;
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

        public string Name
        {
            get { return _name; }
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
    }
}
