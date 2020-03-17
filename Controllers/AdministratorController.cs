using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AsseTS.Models;
using AsseTS.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AsseTS.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly AppDbContext db;
        private readonly IConfiguration configuration;

        public AdministratorController(AppDbContext db, IConfiguration conf)
        {
            this.db = db;
            configuration = conf;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AllUser()
        {
            var users = from u in db.User select u;

            ViewData["allUsers"] = users.ToList();

            return View();
        }

        [Authorize]
        public IActionResult CreateUser()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Submit(User user)
        {
            Console.WriteLine("{0}  {1}", user.Email, user.Password);

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                return Ok("NEGATIVE");

            var existData = from a in db.User select a;
            var existUsername = from n in existData select n.Email;
            if (existUsername.Contains(user.Email))
            {
                return Ok(new
                {
                    ERROR = "Email Exist!"
                });
            }

            user.Password = Hashing.HashPassword(user.Password);

            db.User.Add(user);
            db.SaveChanges();

            return RedirectToAction("AllUser");
        }

        //login redirector//
       

        private User validateLoggedInAdmin()
        {
            if (HttpContext.Session.IsAvailable)
            {
                var adminID = HttpContext.Session.GetInt32("id");

                return db.User.Find(adminID);
            }

            else
                return null;
        }
    }

    public class ViewDataModel
    {
        public User user { get; set; }
        public string role { get; set; }
    }
}