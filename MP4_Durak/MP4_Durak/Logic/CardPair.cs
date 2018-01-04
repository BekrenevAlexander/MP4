using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Api
{
    class CardPair
    {
        int _attack;
        int _defend;

        public CardPair(int attack)
        {
            _attack = attack;
        }

        public int Attack
        {
            get { return _attack; }
        }

        public int Defend
        {
            get { return _defend; }
            set
            {
               _defend = value;
            }
        }

        public bool CheckClose()
        {
            return _attack != 0 && _defend != 0;
        }

        public List<int> GetCards()
        {
            List<int> cards = new List<int>();
            cards.Add(_attack);
            if (_defend != 0)
            {
                cards.Add(_defend);
            }
            return cards;
        } 

    }
}
