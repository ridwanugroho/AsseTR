using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using AsseTS.Models;
using AsseTS.Data;
using System.IO;

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
        public IActionResult Index(string filter, int ? order, string location)
        {

            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            ViewBag.filter = filter;
            ViewBag.order = order;

            var goods = (from g in db.Goods.Include(l=>l.Locations).Include(c=>c.Category).Include(b=>b.Brand) select g).ToList();

            if (!string.IsNullOrEmpty(filter))
                goods = (from g in goods
                        where g.Name.Contains(filter) ||
                        g.Locations.Name.Contains(filter) ||
                        g.Category.Name.Contains(filter) ||
                        g.Brand.Name.Contains(filter) ||
                        g.Status.Contains(filter) ||
                        g.SerialNumber.Contains(filter)
                        select g).ToList();

            if (order.HasValue)
                goods = orderBy(goods, order.Value);

            ViewData["goods"] = goods;

            return View();
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            var ivt = (from g in db.Goods.Include(b => b.Brand).Include(c => c.Category).Include(r => r.Locations)
                      where g.Id.ToString() == id select g).First();

            ViewData["ivt"] = ivt;

            return View();
        }

        [Authorize]
        public IActionResult AddHistory(string goodsId)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            ViewBag.user = userRole;

            var rooms = from r in db.Rooms select r;
            var ivt = db.Goods.Find(Guid.Parse(goodsId));

            ViewData["ivt"] = ivt;
            ViewData["rooms"] = rooms.ToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitHistory(string goodsId, History history, string location)
        {
            if (string.IsNullOrEmpty(history.InStatus))
                history.InDate = null;
            else
                history.OutDate = null;

            var loc = db.Rooms.Find(Guid.Parse(location));
            history.Room = loc;
            history.InCharge = db.User.Find(Guid.Parse(HttpContext.Session.GetString("id")));

            db.Histories.Add(history);

            var goods = db.Goods.Find(Guid.Parse(goodsId));

            var tempH = goods.Histories;
            tempH.Add(history);

            goods.Histories = tempH;
            goods.Locations = loc;
            
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public static List<Goods> orderBy(List<Goods> goods, int? order)
        {
            switch (order)
            {
                case 1:
                    goods = goods.OrderBy(p => p.Name).ToList();
                    break;

                case 2:
                    goods = goods.OrderByDescending(p => p.Name).ToList();
                    break;

                case 3:
                    goods = goods.OrderBy(p => p.CreatedAt).ToList();
                    break;

                case 4:
                    goods = goods.OrderByDescending(p => p.CreatedAt).ToList();
                    break;

                /*case 5:
                    goods = goods.OrderBy(p => p.JoinDate).ToList();
                    break;

                case 6:
                    goods = goods.OrderByDescending(p => p.JoinDate).ToList();
                    break;*/
            }

            return goods;
        }

    }
}