using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MP4_Durak.Models
{
    public enum SuitName { CLUBS, SPADES, HEARTS, DIAMONDS }
    public enum CardName
    {
        SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
    }
    public enum GameName { FOOL, DEBERTZ }

    class Card //Годится для дурака
    {
        private SuitName cardSuit;
        private CardName theCard;
        private int cardRank;
        private bool isTrump;

        public int CardRank
        {
            get { return (cardRank); }
            set { cardRank = value; }
        }
        public SuitName CardSuit
        {
            get { return (cardSuit); }
        }
        public CardName TheCard
        {
            get { return theCard; }
        }
        public bool IsTrump
        {
            get { return (isTrump); }
        }

        public Card()
        {
        }

        public Card(CardName r, SuitName s, bool t)
        {
            cardRank = (int)r;
            cardSuit = s; isTrump = t;
            theCard = r;
        }
    }
    /*------------------------------------------------------------------*/
    //Карта для дурака такая же, как в базовом классе
    class FoolGameCard : Card
    {
        public FoolGameCard(CardName r, SuitName s, bool t) : base(r, s, t)
        {
        }
    }
    /*------------------------------------------------------------------*/
    class DebertzGameCard : Card
    {
        //Для ДЕБЕРЦА важно сколько карта приносит очков
        private int cardScore;
        public int CardScore { get { return (cardScore); } }

        //Старшинство карт в старшинство карт некозырной масти:
        //туз, десять, король, дама, валет, девять, восемь, семь.
        //Старшинство карт в козырной масти:
        //валет, девять, туз, десять, король, дама, восемь, семь.
        //Счет очков для ДЕБЕРЦа:
        //семь, восемь и девять (исключая козырную) очков не приносят
        //туз — 11 очков                //десять — 10 очков
        //король — 4 очка               //дама — 3 очка
        //валет некозырный — 2 очка     //валет козырный — 20 очков
        //девять козырная — 14 очков
        public DebertzGameCard(CardName r, SuitName s, bool t) : base(r, s, t)
        {
            if (t)
            {
                switch (r)
                {
                    case CardName.QUEEN: CardRank = 3; cardScore = 3; break;
                    case CardName.KING: CardRank = 4; cardScore = 4; break;
                    case CardName.TEN: CardRank = 5; cardScore = 10; break;
                    case CardName.ACE: CardRank = 6; cardScore = 11; break;
                    case CardName.NINE: CardRank = 7; cardScore = 14; break;
                    case CardName.JACK: CardRank = 8; cardScore = 20; break;
                    default: CardRank = base.CardRank; cardScore = 0; break;
                }
            }
            else
            {
                switch (r)
                {
                    case CardName.NINE: CardRank = 3; cardScore = 3; break;
                    case CardName.JACK: CardRank = 4; cardScore = 2; break;
                    case CardName.QUEEN: CardRank = 5; cardScore = 3; break;
                    case CardName.KING: CardRank = 6; cardScore = 4; break;
                    case CardName.TEN: CardRank = 7; cardScore = 10; break;
                    case CardName.ACE: CardRank = 8; cardScore = 11; break;
                    default: CardRank = base.CardRank; cardScore = 0; break;
                }
            }
        }
    }
    /*------------------------------------------------------------------*/
    //Класс колода карт
    class CardsPack
    {
        //Здесь собственно и хранится колода
        private List<Card> pack = new List<Card>();
        public List<Card> Pack { get { return (pack); } }
        //Размер колоды
        public int PackSize
        {
            get { return (pack.Count - 1); }
        }

        //В конструкторе инициализируется колода для заданной игры
        //и устанавливается козырная масть
        public CardsPack(GameName game, SuitName trump)
        {
            if (game == GameName.FOOL)
            {
                for (int suit = 0; suit <= 3; suit++)
                {
                    for (int card = 0; card <= 8; card++)
                        pack.Add(new Card((CardName)card,
                          (SuitName)suit, ((SuitName)suit == trump ? true : false)));
                }
            }
            if (game == GameName.DEBERTZ)
            {
                for (int suit = 0; suit <= 3; suit++)
                {
                    for (int card = 1; card <= 8; card++)
                        pack.Add(new DebertzGameCard((CardName)card,
                          (SuitName)suit, ((SuitName)suit == trump ? true : false)));
                }
            }
        }

        //Печать колоды
        //public void PrintPack()
        //{
        //    return 
        //       pack.FindIndex(card => (card.CardSuit == s) && (card.TheCard == c));
        //}

        //Поиск позиции заданной карты в колоде
        public int FindCardInPack(CardName c, SuitName s)
        {
            return
              pack.FindIndex(card => (card.CardSuit == s) && (card.TheCard == c));
        }

        //Вытаскиваем верхнюю карту из колоды
        public Card TakeTopCardFromPack()
        {
            if (PackSize > 0)
            {
                Card c = new Card();
                c = pack[PackSize];
                pack.RemoveAt(PackSize);
                return c;
            }
            else return null;
        }

        //Вытаскиваем заданную карту из колоды
        public Card TakeCardFromPack(CardName card, SuitName suit)
        {
            int i;
            if ((PackSize > 0) && ((i = FindCardInPack(card, suit)) > 0))
            {
                Card c = new Card();
                c = pack[i];
                pack.RemoveAt(i);
                return c;
            }
            else return null;
        }

        //Тасуем колоду
        public void ShufflePack()
        {
            pack = pack.OrderBy(e => Guid.NewGuid()).ToList<Card>();
        }
    }
}