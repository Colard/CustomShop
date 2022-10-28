using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CustomShop.Models;

namespace CustomShop.Controllers
{
    public class GoodsController : Controller
    {
        // GET: Goods
        private ShopContext db = new ShopContext();

        public ActionResult Index(string filter = null, int? min = null, int? max = null)
        {
            var listOfGoods = db.Goods.ToList();
            listOfGoods.Reverse();
            return View(listOfGoods);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sizes = db.Sizes;
            good.Price = Math.Round(good.Price);
            good.GoodType = db.GoodTypes.Find(good.GoodTypeId);
            ViewBag.Colors = db.Colors.Where(el => el.GoodId == good.Id);
            return View(good);
        }

    }
}