using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using AsseTS.Models;
using AsseTS.Data;

namespace AsseTS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        private readonly IConfiguration configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext db, IConfiguration conf, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.db = db;
            configuration = conf;
        }


            
        public IActionResult Index()
        {
            var user = validateLoggedIn();
            if (user != null)
            {
                ViewData["user"] = user;
                return View("Redirector");
            }

            else
                return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var _user = AuthenticatedUser(user);

            if (_user == null)
            {
                ViewData["loginInfo"] = "Email/ Password salah";
                return View("Index");
            }

            var token = generateJwtToken(_user);

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("id", _user.Id.ToString());

            ViewData["user"] = _user;

            return View("Redirector");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        private string generateAdminCode(int ln)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[ln];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            return new String(stringChars);
        }

        private string generateJwtToken(User admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                // issuer: Configuration["Jwt:Issuer"],
                // audience: Configuration["Jwt:Audience"],
                null,
                null,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        public User AuthenticatedUser(User user_input)
        {
            var user = from _user in db.User where _user.Email == user_input.Email && _user.DataStatus != 0 select _user;

            if (user.FirstOrDefault() != null)
            {
                if (Hashing.ValidatePassword(user_input.Password, user.First().Password))
                    return user.First();
            }

            return null;
        }

        private User validateLoggedIn()
        {
            var userID = HttpContext.Session.GetString("id");
            if (HttpContext.Session.IsAvailable && !string.IsNullOrEmpty(userID))
            {
                return db.User.Find(Guid.Parse(userID));
            }

            else
                return null;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
