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
        public List<int> GetCards(Guid gameId)
        {
            Game game=RoomsService.GetInstance().GetGame(gameId);
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

        public bool GetAttacker(Guid gameId)
        {
            return RoomsService.GetInstance().GetGame(gameId).IsFirstPlayerAttack;
        }

        public int GetTrump(Guid gameId)
        {
            return RoomsService.GetInstance().GetGame(gameId).Trump;
        }

        public void Attack(Guid gameId, int card)
        {
            RoomsService.GetInstance().GetGame(gameId).Attack(card);
        }

        public void Defend(Guid gameId, int attackCard, int defendCard)
        {
            RoomsService.GetInstance().GetGame(gameId).Defend(attackCard, defendCard);
        }

        public void DefenderGetCards(Guid gameId)
        {
            RoomsService.GetInstance().GetGame(gameId).DefenderGetCards();
        }

        public void DefenderWinRound(Guid gameId)
        {
            RoomsService.GetInstance().GetGame(gameId).DefenderWinRound();
        }

    }
}