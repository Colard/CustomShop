using CustomShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CustomShop.Controllers
{
    public class AdminController : Controller
    {
        ShopContext db = new ShopContext();
        // GET: Admin
        public ActionResult Index()
        {
            var list = db.Purchases.Include(el => el.Client).ToList();
            list.Reverse();
            return View(list);
        }
    }
}