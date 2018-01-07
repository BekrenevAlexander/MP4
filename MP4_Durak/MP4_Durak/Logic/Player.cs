using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Api
{
    public class Player
    {
        List<int> _cards;
        Guid id;



        public Player(List<int> cards,Guid id)
        {
            _cards = cards;
            this.id=id;
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
    }
}
