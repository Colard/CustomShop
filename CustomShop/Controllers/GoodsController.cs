using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CustomShop.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
namespace CustomShop.Controllers
{
    public class GoodsController : Controller
    {
        // GET: Goods
        private ShopContext db = new ShopContext();

        public ActionResult Index(string filter = null, int? min = null, int? max = null, string searchText = null)
        {
            ViewBag.Filter = filter ?? "Усі товари";
            ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.SearchText = searchText;
            IQueryable<Good> listOfGoods = db.Goods;

            if (!String.IsNullOrEmpty(searchText))
            {
                listOfGoods = listOfGoods.Where(el => el.Name.ToLower().IndexOf(searchText.ToLower()) >= 0);
            }

            if (min != null)
            {
                listOfGoods = listOfGoods.Where(el => el.Price >= min);
            }

            if (max != null)
            {
                listOfGoods = listOfGoods.Where(el => el.Price <= max);
            }

            if (!String.IsNullOrEmpty(filter))
                switch (filter.ToLower())
                {
                    case "худі та світшоти": listOfGoods = listOfGoods.Where(el => el.GoodTypeId == 1 || el.GoodTypeId == 2); break;
                    case "футболки": listOfGoods = listOfGoods.Where(el => el.GoodTypeId == 3); break;
                    case "штани та шорти": listOfGoods = listOfGoods.Where(el => el.GoodTypeId == 4 || el.GoodTypeId == 5); break;
                    case "аксесуари": listOfGoods = listOfGoods.Where(el => el.GoodTypeId == 6); break;
                }


            var filteredlistOfGoods = listOfGoods.ToList();
            filteredlistOfGoods.Reverse();
            return View(filteredlistOfGoods);
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