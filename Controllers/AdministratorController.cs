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
using Microsoft.EntityFrameworkCore;

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
            var users = from u in db.User select u;

            ViewData["users"] = users.ToList();

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
        public IActionResult Submit(User user, Address addr)
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

            user.Address = addr;

            db.User.Add(user);
            db.SaveChanges();

            return RedirectToAction("AllUser");
        }

        [Authorize]
        public IActionResult UserDetail(string id)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            var _user = db.User.Find(Guid.Parse(id));

            ViewData["usr"] = _user;

            return View();
        }

        [Authorize]
        public IActionResult Update(string id)
        {
            var update = db.User.Find(Guid.Parse(id));

            ViewData["update"] = update;

            return View();
        }

        [Authorize]
        public IActionResult SubmitUpdate(User user, Address addr)
        {
            var tempU = db.User.Find(user.Id);

            user.Password = tempU.Password;

            var propName = typeof(User).GetProperties();

            foreach (var n in propName)
            {
                Console.WriteLine("set : {0}", n.ToString());
                var val = n.GetValue(user, null);
                n.SetValue(tempU, val);
            }

            tempU.Address = addr;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [Authorize]
        public IActionResult Remove(string id)
        {
            var rmv = db.User.Find(Guid.Parse(id));

            rmv.DataStatus = 0;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Restore(string id)
        {
            var rmv = db.User.Find(Guid.Parse(id));

            rmv.DataStatus = 1;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

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