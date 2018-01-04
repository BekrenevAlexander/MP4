using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Api
{
    class CardUtil
    {
        public static bool CheckForDuplicateCards(List<int> cards, int checkCard)
        {
            return cards.Any(card => card == checkCard);
        }
        public static bool CheckForDuplicateCards(List<int> cards, List<int> checkCards)
        {
            return cards.Any(card => checkCards.Any(t=>t==card));
        }
    }
}
