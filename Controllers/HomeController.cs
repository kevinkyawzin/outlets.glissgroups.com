using BigMac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigMac.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

        //    return View();
        //}

        public ActionResult Index()
        {
            callDefaultAppFields();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void callDefaultAppFields()
        {
            AppSettings settings = new AppSettings();
            ViewBag.favicon = settings.favicon;
            ViewBag.logo = settings.logo;
            ViewBag.home = settings.home;
            ViewBag.appName = settings.appName;
            ViewBag.contact = settings.contact;

        }
    }
}
