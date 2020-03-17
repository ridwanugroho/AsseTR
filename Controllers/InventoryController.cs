using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AsseTS.Models;
using AsseTS.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AsseTS.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDbContext db;

        public InventoryController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            var goods = from g in db.Goods select g;
            ViewData["goods"] = goods.ToList();

            return View();
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            var ivt = from g in db.Goods.Include(b => b.Brand).Include(c => c.Category).Include(r => r.Locations) 
                      where g.Id.ToString() == id select g;

            ViewData["ivt"] = ivt.First();

            return View();
        }

        [Authorize]
        public IActionResult AddHistory(string goodsId)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;

            var rooms = from r in db.Rooms select r;
            var ivt = db.Goods.Find(Guid.Parse(goodsId));

            ViewData["ivt"] = ivt;
            ViewData["rooms"] = rooms.ToList();
            ViewBag.user = userRole;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitHistory(string goodsId, History history, string location)
        {
                
            var loc = db.Rooms.Find(Guid.Parse(location));
            history.Room = loc;

            db.Histories.Add(history);

            var goods = db.Goods.Find(Guid.Parse(goodsId));

            var tempH = goods.Histories;
            tempH.Add(history);

            goods.Histories = tempH;
            goods.Locations = loc;
            
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}