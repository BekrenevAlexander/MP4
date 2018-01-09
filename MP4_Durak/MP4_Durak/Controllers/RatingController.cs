using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using Durak.Api;
using Microsoft.AspNet.Identity;
using MP4_Durak.Logic;
using MP4_Durak.Models;

namespace MP4_Durak.Controllers
{
    public class RatingController : ApiController
    {

        [HttpGet]
        [ActionName("getRating")]
        public List<ApplicationUser> GetRating()
        {
            var context = ApplicationDbContext.Create();
            var usersContext = context.Users;
            usersContext.Load();
            return usersContext.Local.ToList();
        }
        [HttpGet]
        [ActionName("getRatingSort")]
        public List<ApplicationUser> getRatingSort()
        {
            var context = ApplicationDbContext.Create();
            var usersContext = context.Users;
            usersContext.Load();
            var users = usersContext.Local.ToList();
            users.Sort((x, y) =>
                    y.Wins.CompareTo(x.Wins));
            return users;
        }
        private static int CompareLoses(ApplicationUser x, ApplicationUser y)
        {
            if (x.Games - x.Wins > y.Games - y.Wins)
                return -1;
            else
                return 1;
            
        }
        [HttpGet]
        [ActionName("getRatingSortByLose")]
        public List<ApplicationUser> getRatingSortByLose()
        {
            var context = ApplicationDbContext.Create();
            var usersContext = context.Users;
            usersContext.Load();
            var users = usersContext.Local.ToList();
            users.Sort(CompareLoses);
            return users;
        }
        private static int ComparePercent(ApplicationUser x, ApplicationUser y)
        {
            if ((float)x.Wins / x.Games > (float)y.Wins / y.Games)
                return -1;
            else
                return 1;

        }
        [HttpGet]
        [ActionName("getRatingSortByPercent")]
        public List<ApplicationUser> getRatingSortByPercent()
        {
            var context = ApplicationDbContext.Create();
            var usersContext = context.Users;
            usersContext.Load();
            var users = usersContext.Local.ToList();
            users.Sort(ComparePercent);
            return users;
        }
    }
}