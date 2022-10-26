using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomShop.Models;

namespace CustomShop.Controllers
{   
    public class HomeController : Controller
    {
        ShopContext db = new ShopContext();
        public ActionResult Index()
        {
            return View(db.Sizes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}