using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AsseTS.Models;
using AsseTS.Data;
using Microsoft.AspNetCore.Authorization;

namespace AsseTS.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext db;

        public ManagerController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var cats = from c in db.Categories select c;
            var rooms = from r in db.Rooms select r;
            var brands = from b in db.Brands select b;

            ViewData["brands"] = brands.ToList();
            ViewData["cats"] = cats.ToList();
            ViewData["rooms"] = rooms.ToList();

            return View();
        }

        [Authorize]
        public IActionResult AddCategory()
        {
            return View();
        }

        [Authorize]
        public IActionResult SubmitCategory(Category cat)
        {
            db.Categories.Add(cat);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult AddRoom()
        {
            return View();
        }

        [Authorize]
        public IActionResult SubmitRoom(Room room)
        {
            db.Rooms.Add(room);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult AddBrand()
        {
            return View();
        }

        [Authorize]
        public IActionResult SubmitBrand(Brand brand)
        {
            db.Brands.Add(brand);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult UpdateRoom(string id)
        {
            var room = db.Rooms.Find(Guid.Parse(id));
            ViewData["room"] = room;

            return View();
        }

        [Authorize]
        public IActionResult SubmitRoomUpdate(Room room)
        {
            var tempR = db.Rooms.Find(room.Id);

            var propName = typeof(Room).GetProperties();

            room.CreatedAt = tempR.CreatedAt;
            room.EditedAt = DateTime.Now;

            foreach (var n in propName)
            {
                Console.WriteLine("set : {0}", n.ToString());
                var val = n.GetValue(room, null);
                n.SetValue(tempR, val);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}