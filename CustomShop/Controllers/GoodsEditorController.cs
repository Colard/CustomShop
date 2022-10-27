using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CustomShop.Models;
using Microsoft.Ajax.Utilities;

namespace CustomShop.Controllers
{
    public class GoodsEditorController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: GoodsEditor
        public ActionResult Index(string searchText, int? GoodTypeId)
        {
            var goods = db.Goods.Include(g => g.GoodType);
            var goodTypes = db.GoodTypes.ToList();

            if (!String.IsNullOrEmpty(searchText))
            {
                goods = goods.Where(p => p.Name.ToLower().IndexOf(searchText.ToLower()) >=0);
            }

            if (GoodTypeId != null && GoodTypeId != 0)
            {
                goods = goods.Where(p => p.GoodType.Id == GoodTypeId);
            }

            goodTypes.Insert(0, new GoodType { Name = "Всі", Id = 0 });
            ViewBag.GoodTypeId = new SelectList(goodTypes, "Id", "Name", 0);

            var goodsList = goods.ToList();
            goodsList.Sort((el1, el2) => el1.Name.CompareTo(el2.Name));
            return View(goodsList);
        }

        // GET: GoodsEditor/Details/5
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
            good.Price = Math.Round(good.Price);
            good.GoodType = db.GoodTypes.Find(good.GoodTypeId);
            ViewBag.Colors = db.Colors.Where(el => el.GoodId == good.Id);
            return View(good);
        }

        // GET: GoodsEditor/Create
        public ActionResult Create()
        {
            ViewBag.GoodTypeId = new SelectList(db.GoodTypes, "Id", "Name");
            return View();
        }

        // POST: GoodsEditor/Create
        [HttpPost]
        public ActionResult Create(Good good, string colorslist, HttpPostedFileBase uploadImage)
        {
            if (!ModelState.IsValid || uploadImage == null)
            {
                ViewBag.GoodTypeId = new SelectList(db.GoodTypes, "Id", "Name", good.GoodTypeId);
                return View(good);
            }

            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }
            good.Image = imageData;

            db.Goods.Add(good);

            string[] colorslistArr = colorslist.Replace(" ", string.Empty).Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            foreach(string color in colorslistArr)
            {
                db.Colors.Add(new Color { Code = color, GoodId = good.Id});
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GoodsEditor/Edit/5
        public ActionResult Edit(int? id)
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
            var colorTextArr = db.Colors.Where(el => el.GoodId == id).ToArray();
            string colorText = "";
            foreach (var color in colorTextArr) {
                colorText += color.Code + ", ";
            }
            
            ViewBag.ColorsList = colorText;
            ViewBag.GoodTypeId = new SelectList(db.GoodTypes, "Id", "Name", good.GoodTypeId);
            return View(good);
        }

        // POST: GoodsEditor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Good good, string colorslist, HttpPostedFileBase uploadImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GoodTypeId = new SelectList(db.GoodTypes, "Id", "Name", good.GoodTypeId);
                return View(good);
            }

            if(uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                good.Image = imageData;
            } 
            else
            {
                Good OldGood = db.Goods.AsNoTracking().Where((el) => el.Id == good.Id).ToArray()[0];
                good.Image = OldGood.Image;
            }

            var colors = db.Colors.Where(el => el.GoodId == good.Id);
            foreach (var color in colors) {
                color.GoodId = null;
                db.Entry(color).State = EntityState.Modified;
            }

            string[] colorslistArr = colorslist.Replace(" ", string.Empty).Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            foreach (string color in colorslistArr)
            {
                db.Colors.Add(new Color { Code = color, GoodId = good.Id });
            }

            db.Entry(good).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: GoodsEditor/Delete/5
        public ActionResult Delete(int? id)
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

            db.Goods.Remove(good);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
