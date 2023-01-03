﻿using System.Web.Mvc;

namespace WebAppDotnetFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult FCN()
        {
            return View();
        }

        public ActionResult Uptime()
        {
            return View(WebApiApplication.Started);
        }
    }
}
