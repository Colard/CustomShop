using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomShop.Controllers
{
    public class GoodsController : Controller
    {
        // GET: Goods
        public ActionResult Index(string filter = null, int? min = null, int? max = null)
        {
            return View();
        }
    }
}