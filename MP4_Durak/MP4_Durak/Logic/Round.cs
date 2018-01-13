using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Api
{
    public class Round
    {
        int _trump;
        List<CardPair> _cardPairs;
        readonly int _maxPairs;

        public Round(int maxPairs, int trump,int attackCard)
        {
            _maxPairs = maxPairs;
            _trump = trump;
            _cardPairs = new List<CardPair>();
            _cardPairs.Add(new CardPair(attackCard));
        }
        private bool CheckExistAttackCard(int card)
        {
            return _cardPairs.Any(t => t.Attack % 10 == card % 10 || t.Defend % 10 == card % 10);
        }

        private CardPair FindCardPair(int attackCard)
        {
            return _cardPairs.Find(t => t.Attack == attackCard);
        }
        public void Attack(int card)
        {
            if (_cardPairs.Count == _maxPairs)
            {
                throw new Exception("Больше подкидывать нельзя");
            }
            if (CheckExistAttackCard(card))
            {
                _cardPairs.Add(new CardPair(card));
            }
            else
            {
                throw new Exception("Нельзя подкидывать эту карту, потому что на столе нет карты с таким значением!");
            }
        }

        public void Defend(int attackCard, int defendCard)
        {
            CardPair cardPair;
            try
            {
                cardPair = FindCardPair(attackCard);
            }
            catch(Exception e)
            {
                throw new Exception("Нет карты на столе, которую вы пытаетесь побить",e);
            }

            if (cardPair.Attack/10 == _trump)
            {
                //Проверка на то, что козырь защищающегося больше козыря атакующего
                if (defendCard/10 == _trump && defendCard%10 > cardPair.Attack%10)
                {
                    cardPair.Defend = defendCard;
                }
                else
                {
                    throw new Exception("Вы не можете побить этой картой эту карту");
                }
            }
            else
            {
                //Если бьет козырем обычную карту
                if (defendCard/10 == _trump)
                {
                    cardPair.Defend = defendCard;
                }
                else
                {
                    //Если бьет обычной картой обычную карту
                    if (defendCard/10 == cardPair.Attack/10 && defendCard%10 > cardPair.Attack%10)
                    {
                        cardPair.Defend = defendCard;
                    }
                    else
                    {
                        throw new Exception("Вы не можете побить этой картой эту карту");
                    }
                }
            }



        }

        public bool CheckAllCardsArePaired()
        {
            return _cardPairs.All(t => t.CheckClose());
        }

        public List<int> GetAllCards()
        {
            List<int> cards = new List<int>();
            foreach (CardPair cardPair in _cardPairs)
            {
                cards.AddRange(cardPair.GetCards());
            }
            return cards;
        }
        public List<CardPair> GetAllCardsOnTable()
        {
            return _cardPairs;
        }
    }
}
