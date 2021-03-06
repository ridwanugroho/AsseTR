﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AsseTS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnauthorizedCustom()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult BadRequestCustom()
        {
            return View();
        }
    }
}