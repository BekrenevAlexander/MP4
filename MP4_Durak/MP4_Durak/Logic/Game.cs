using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.Api
{
    public class Game
    {
        static readonly int _cardsColodeSize = 36;

        Player _player1;
        Player _player2;
        int _trump;
        Queue<int> _cards;

        Round _currentRound;

        DateTime lastActionTime;
        Player lastActionPlayer;

        bool _isFirstRound;
        bool _isFirstPlayerAttack;
        public Game(Guid firstPlayerId,Guid secondPlayerId)
        {
            _cards = InitCardsColode();
            _player1=new Player(GetNextCards(6), firstPlayerId);
            _player2 = new Player(GetNextCards(6), secondPlayerId);
            _trump = GetTrump();
            Random random = new Random();
            _isFirstPlayerAttack = random.Next()%2 == 0;
            _isFirstRound = true;
            lastActionTime=DateTime.Now;
        }

        public Player LastActionPlayer
        {
            get { return lastActionPlayer; }
            set { lastActionPlayer = value; }
        }

        public DateTime LastActionTime
        {
            get { return lastActionTime; }
            set { lastActionTime = value; }
        }

        public static int CardsColodeSize
        {
            get { return _cardsColodeSize; }
        }

        public int Trump
        {
            get { return _trump; }
        }

        public bool IsFirstRound
        {
            get { return _isFirstRound; }
        }

        public bool IsFirstPlayerAttack
        {
            get { return _isFirstPlayerAttack; }
        }

        private Queue<int> InitCardsColode()
        {
            List<int> cards= new List<int>();
            for (int i = 0; i <4; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    cards.Add(j+i*10);
                }
            }
            Random random = new Random();
            int buffIndex1;
            int buffIndex2;
            int buffer;
            for (int i = 0; i < 200; i++)
            {
                buffIndex1 = random.Next(_cardsColodeSize);
                buffIndex2 = random.Next(_cardsColodeSize);
                buffer = cards[buffIndex1];
                cards[buffIndex1] = cards[buffIndex2];
                cards[buffIndex2] = buffer;
            }
            return new Queue<int>(cards);
        }

        private List<int> GetNextCards(int count)
        {
            List<int> cards = new List<int>();
            for (int i = 0; i < count; i++)
            {
                if (_cards.Count > 0)
                {
                    cards.Add(_cards.Dequeue());
                }
            }
            return cards;
        }

        public int GetTrump()
        {
            return _cards.Peek()/10;
        }

        private void EndRound(bool firstFirstPlayer)
        {
            _currentRound = null;
            if (firstFirstPlayer)
            {
                _player1.AddCards(GetNextCards(6 - _player1.Cards.Count));
                _player2.AddCards(GetNextCards(6 - _player2.Cards.Count));
            }
            else
            {
                _player2.AddCards(GetNextCards(6 - _player2.Cards.Count));
                _player1.AddCards(GetNextCards(6 - _player1.Cards.Count));
            }
            
        }
        public Player GetAttacker()
        {
            return _isFirstPlayerAttack ? _player1 : _player2;
        }
        public Player GetDefender()
        {
            return _isFirstPlayerAttack ? _player2 : _player1;
        }

        public void Attack(int card)
        {
            if (!GetAttacker().CheckExistCard(card))
            {
                throw new Exception("У атакующего нет такой карты");
            }

            if (_currentRound == null)
            {
                _currentRound= new Round(_isFirstRound?5:6,_trump,card);
            }
            else
            {
                _currentRound.Attack(card);
            }
            GetAttacker().RemoveCard(card);
            lastActionPlayer = GetAttacker();
            lastActionTime=DateTime.Now;
        }

        public void Defend(int attackCard, int defendCard)
        {
            if (!GetDefender().CheckExistCard(defendCard))
            {
                throw new Exception("У защищающегося нет такой карты");
            }

            _currentRound.Defend(attackCard, defendCard);
            GetDefender().RemoveCard(defendCard);
            lastActionPlayer = GetDefender();
            lastActionTime = DateTime.Now;
        }

        public bool DefenderGetCards()
        {
            GetDefender().AddCards(_currentRound.GetAllCards());
            EndRound(_isFirstPlayerAttack);
            lastActionPlayer = GetDefender();
            lastActionTime = DateTime.Now;
            return GetAttacker().Cards.Count == 0 || GetDefender().Cards.Count == 0;
        }

        public bool DefenderWinRound()
        {
            _isFirstRound = false;
            EndRound(_isFirstPlayerAttack);
            _isFirstPlayerAttack = !_isFirstPlayerAttack;
            lastActionPlayer = GetAttacker();
            lastActionTime = DateTime.Now;
            return GetAttacker().Cards.Count == 0|| GetDefender().Cards.Count == 0;
        }

        public bool GetWhoWin()
        {
            if (_player1.Cards.Count == 0)
                return true;
            if (_player2.Cards.Count == 0)
                return false;
            throw new Exception("Никто не победил");
        }

        public List<string> GetMessages(Guid playerId)
        {
            if (_player1.Id.Equals(playerId))
            {
                return _player2.GetMessages();
            }
            if (_player2.Id.Equals(playerId))
            {
                return _player1.GetMessages();
            }
            throw new Exception("Нет такого пользователя");
        }

        public void AddMessage(Guid playerId,string message)
        {
            if (_player1.Id.Equals(playerId))
            {
                _player1.AddMessage(message);
            }
            if (_player2.Id.Equals(playerId))
            {
                _player2.AddMessage(message);
            }
        }
    }
}
