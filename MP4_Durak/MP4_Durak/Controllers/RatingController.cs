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
    }
}