using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BarcodeLib;
using System.Drawing;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;

using AsseTS.Models;
using AsseTS.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AsseTS.Controllers
{
    public class OperatorController : Controller
    {
        private readonly AppDbContext db;

        public OperatorController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Inventory");
        }

        [Authorize]
        public IActionResult AddGoods()
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;

            var cats = from c in db.Categories select c;
            var rooms = from r in db.Rooms select r;
            var brands = from b in db.Brands select b;

            ViewData["cats"] = cats.ToList();
            ViewData["rooms"] = rooms.ToList();
            ViewData["brands"] = brands.ToList();
            ViewBag.user = userRole;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> SubmitGoods(Goods goods, string brand, string location, string category, [FromForm(Name = "img")] List<IFormFile> img)
        {
            var path = "wwwroot/img/";
            var filename = new List<string>();
            Directory.CreateDirectory(path);

            foreach(var i in img)
            {
                if (i != null)
                {
                    var fn = Path.Combine(path, Path.GetRandomFileName() + ".jpg");
                    using (var stream = new FileStream(fn, FileMode.Create))
                    {
                            await i.CopyToAsync(stream);
                    }

                    filename.Add(fn.Substring(8));
                }
            }

            var _brand = db.Brands.Find(Guid.Parse(brand));
            var cat = db.Categories.Find(Guid.Parse(category));
            var room = db.Rooms.Find(Guid.Parse(location));
            goods.Locations = room;
            goods.Brand = _brand;
            goods.Category = cat;
            goods.Images = filename;
            goods.Barcode = generateBarcode(goods.SerialNumber);
            
            db.Goods.Add(goods);

            db.SaveChanges();

            return RedirectToAction("Index", "Inventory");
        }

        [Authorize]
        public IActionResult UpdateGoods(string goodsId)
        {
            var userId = HttpContext.Session.GetString("id");
            var userRole = db.User.Find(Guid.Parse(userId)).Role;
            var cats = from c in db.Categories select c;
            var rooms = from r in db.Rooms select r;
            var brands = from b in db.Brands select b;

            ViewData["cats"] = cats.ToList();
            ViewData["rooms"] = rooms.ToList();
            ViewData["brands"] = brands.ToList();

            var goods = db.Goods.Find(Guid.Parse(goodsId));

            ViewData["item"] = goods;
            ViewBag.user = userRole;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Update(Goods goods, string brand, string location, string category, List<string> prevImg, [FromForm(Name = "img")] List<IFormFile> img)
        {
            var tempG = db.Goods.Find(goods.Id);


            goods.CreatedAt = tempG.CreatedAt;
            goods.Histories = tempG.Histories;
            goods.Barcode = tempG.Barcode;
            var _brand = db.Brands.Find(Guid.Parse(brand));
            var cat = db.Categories.Find(Guid.Parse(category));
            var room = db.Rooms.Find(Guid.Parse(location));
            goods.Locations = room;
            goods.Brand = _brand;
            goods.Category = cat;

            var propName = typeof(Goods).GetProperties();

            foreach (var n in propName)
            {
                Console.WriteLine("set : {0}", n.ToString());
                var val = n.GetValue(goods, null);
                n.SetValue(tempG, val);
            }

            var path = "wwwroot/img/";
            Directory.CreateDirectory(path);

            if(img != null)
            {
                for (int i=0; i<img.Count(); i++)
                {
                    if(img[i] != null)
                    {
                        var fn = Path.Combine(path, Path.GetRandomFileName() + ".jpg");
                        using (var stream = new FileStream(fn, FileMode.Create))
                        {
                            await img[i].CopyToAsync(stream);
                        }

                        prevImg[i] = fn.Substring(8);
                    }
                }
            }

            tempG.Images = prevImg;

            tempG.EditedAt = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index", "Inventory");
        }

        private string generateBarcode(string sn)
        {
            Barcode barcodeAPI = new Barcode();

            int imageWidth = 290;
            int imageHeight = 120;
            Color foreColor = Color.Black;
            Color backColor = Color.Transparent;
            string data = sn;

            Image barcodeImage = barcodeAPI.Encode(TYPE.CODE128, data, foreColor, backColor, imageWidth, imageHeight);
            var path = Path.Combine("wwwroot/barcode/", Path.GetRandomFileName() + ".png");

            barcodeImage.Save(path, ImageFormat.Png);

            return path.Substring(8);
        }
    }
}