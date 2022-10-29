using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomShop.Models;
using System.Diagnostics;
namespace CustomShop.Controllers
{
    public class PurchasesController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Purchases
        public ActionResult Index(int? code = null, string searchText = null, int? filter = null)
        {
            var purchases = db.Purchases.Include(p => p.Client).Include(p => p.PurchaseState);
            
            ViewBag.Filter = filter;
            ViewBag.Code = code;
            ViewBag.SearchText = searchText;

            decimal money = 0;
            var complitedPurchases = db.Purchases
                .Include(p => p.PurchaseState)
                .Where(el => el.PurchaseStateId == 3 && el.Date.Year == DateTime.Now.Year && el.Date.Month == DateTime.Now.Month);

            foreach (var item in complitedPurchases) money += item.Price;

            ViewBag.Money = money;
            ViewBag.PurchaseCount = complitedPurchases.Count();
            ViewBag.NewPurchaseCount = db.Purchases.Include(p => p.PurchaseState).Where(el => el.PurchaseStateId == 1).Count();

            if (!String.IsNullOrEmpty(searchText))
            {
                purchases = purchases.Where(el => 
                    el.Client.Name.ToLower().IndexOf(searchText.ToLower()) >= 0 ||
                    el.Client.Surname.ToLower().IndexOf(searchText.ToLower()) >= 0 ||
                    el.Client.LastName.ToLower().IndexOf(searchText.ToLower()) >= 0
                );
            }

            if (code != null)
            {
                purchases = purchases.Where(el => el.Id == code);
            }

            if (filter != null)
                switch (filter)
                {
                    case 1: purchases = purchases.Where(el => el.PurchaseStateId == 1); break;
                    case 2: purchases = purchases.Where(el => el.PurchaseStateId == 2); break;
                    case 3: purchases = purchases.Where(el => el.PurchaseStateId == 3); break;
                }

            var purchasesList = purchases.ToList();
            purchasesList.Reverse();

            return View(purchasesList);
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }

            purchase.Client = db.Clients.Find(purchase.ClientId);
            purchase.Client.Post = db.Posts.Find(purchase.Client.PostId);

            purchase.PurchaseState = db.PurchaseStates.Find(purchase.PurchaseStateId);

            purchase.Cart = db.Carts.Include(el => el.Good).Include(el => el.Size).Include(el => el.Color).Where(el => el.PurchaseId == purchase.Id);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseStateId = new SelectList(db.PurchaseStates, "Id", "Name", purchase.PurchaseStateId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Price,Date,ClientId,PurchaseStateId")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", purchase.ClientId);
            ViewBag.PurchaseStateId = new SelectList(db.PurchaseStates, "Id", "Name", purchase.PurchaseStateId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }

            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteGoodFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }

            Purchase purchase = db.Purchases.Find(cart.PurchaseId);

            db.Carts.Remove(cart);

            decimal newPrice = 0;
            var otherCarts = db.Carts.Include(el => el.Good).Where(el => el.PurchaseId == purchase.Id);
            
            foreach(var otherCart in otherCarts)
            {
                if(otherCart.Good != null) newPrice += otherCart.Good.Price;
            }

            purchase.Price = newPrice;

            db.Entry(purchase).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Details", "Purchases", new { id = purchase.Id });
        }

        public ActionResult EditGoodFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }

            cart.Good = db.Goods.Find(cart.GoodId);
            
            cart.Good.Colors = db.Colors.Where(el => el.GoodId == cart.GoodId);
            
            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name", cart.SizeId);
            
            return View(cart);
        }

        [HttpPost]
        public ActionResult EditGoodFromCart(Cart cart) {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = cart.PurchaseId });
            }

            cart.Good = db.Goods.Find(cart.GoodId);

            cart.Good.Colors = db.Colors.Where(el => el.GoodId == cart.GoodId);

            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name", cart.SizeId);

            return View(cart);
        }

        public ActionResult EditClient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", client.PostId);
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                Purchase purchase = db.Purchases.Where( el => el.ClientId == client.Id).ToList()[0];
                return RedirectToAction("Details", new { id = purchase.Id });
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", client.PostId);
            return View(client);
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
