using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Durak.Api;
using Microsoft.AspNet.Identity;
using MP4_Durak.Logic;
using MP4_Durak.Models;

namespace MP4_Durak
{
    public class GameController : ApiController
    {

        [HttpGet]
        [ActionName("getCards")]
        public List<int> GetCards(string gameId)
        {
            Game game=RoomsService.GetInstance().GetGame(Guid.Parse(gameId));
            if (game.GetAttacker().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                return game.GetAttacker().Cards;
            }
            if (game.GetDefender().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                return game.GetDefender().Cards;
            }
            return null;
        }

        [HttpGet]
        [ActionName("getAllCardsOnTable")]
        public List<CardPair> GetCardsOnTable(string gameId)
        {
            Game game = RoomsService.GetInstance().GetGame(Guid.Parse(gameId));
            return game.GetAllCardsOnTable();
        }

        [HttpGet]
        [ActionName("getEnemyCardsCount")]
        public int GetEnemyCardsCount(string gameId)
        {
            Game game = RoomsService.GetInstance().GetGame(Guid.Parse(gameId));
            if (game.GetAttacker().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                return game.GetAttacker().Cards.Count;
            }
            if (game.GetDefender().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                return game.GetDefender().Cards.Count;
            }
            return -1;
        }

        [HttpGet]
        [ActionName("getAttacker")]
        public bool GetAttacker(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).IsFirstPlayerAttack;
        }

        [HttpGet]
        [ActionName("getTrump")]
        public int GetTrump(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).Trump;
        }

        [HttpGet]
        [ActionName("attack")]
        public void Attack(string gameId, int card)
        {
            RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).Attack(card);
        }

        [HttpGet]
        [ActionName("defend")]
        public void Defend(string gameId, int attackCard, int defendCard)
        {
            RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).Defend(attackCard, defendCard);
        }
        [HttpGet]
        [ActionName("doSmth")]
        public void DoSmth(string gameId, int attackCard, int defendCard)
        {
            Game game = RoomsService.GetInstance().GetGame(Guid.Parse(gameId));
            if (game.GetAttacker().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                game.Attack(attackCard);
            }
            if (game.GetDefender().Id.Equals(Guid.Parse(User.Identity.GetUserId())))
            {
                game.Defend(attackCard, defendCard);
            }
        }

        [HttpGet]
        [ActionName("defenderGetCards")]
        public bool DefenderGetCards(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).DefenderGetCards();
        }

        [HttpGet]
        [ActionName("isRoundEnd")]
        public bool IsRoundEnd(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).GetIsRoundEnd();
        }

        [HttpGet]
        [ActionName("defenderWinRound")]
        public bool DefenderWinRound(string gameId)
        {
           return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).DefenderWinRound();
        }

        [HttpGet]
        [ActionName("getWhoWin")]
        public bool GetWhoWin(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).GetWhoWin();
        }

        [HttpGet]
        [ActionName("getMessages")]
        public List<string> GetMessages(string gameId)
        {
            return RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).GetMessages(Guid.Parse(User.Identity.GetUserId()));
        }

        [HttpPost]
        [ActionName("addMessage")]
        public void AddMessage(string gameId,string message)
        {
            RoomsService.GetInstance().GetGame(Guid.Parse(gameId)).AddMessage(Guid.Parse(User.Identity.GetUserId()),message);
        }
    }
}