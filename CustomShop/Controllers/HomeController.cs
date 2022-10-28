using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomShop.Models;
using System.Text.Json;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace CustomShop.Controllers
{
    public class GoodSettings
    {
        public GoodSettings(Good good, Size size, Color color)
        {
            Good = good;
            Size = size;
            Color = color;
        }

        public Good Good { get; set; }
        public Size Size { set; get; }
        public Color Color { set; get; }
    }

    public class HomeController : Controller
    {
        ShopContext db = new ShopContext();
        public ActionResult Index()
        {
            return View(db.Sizes);
        }

        public ActionResult Cart()
        {
            var text = HttpContext.Request.Cookies["cart"].Value;
            if (text != null) text = HttpUtility.UrlDecode(text);
            List<GoodSettings> goods = new List<GoodSettings>();
            ViewBag.Price = 0;

            try
            {
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new Int32Converter());
                GoodInfo[] obj = JsonSerializer.Deserialize<GoodInfo[]>(text, serializeOptions);

                foreach (GoodInfo item in obj)
                {
                    Good Good = db.Goods.Find(item.goodId);
                    Size Size = db.Sizes.Find(item.sizeId);
                    Color Color = db.Colors.Find(item.colorId);
                    if (Color.GoodId != item.goodId) Color = null;

                    if (Good != null || Size != null || Color != null)
                    {
                        ViewBag.Price += Good.Price;
                        goods.Add(new GoodSettings(Good, Size, Color));
                    }
                }
            }
            catch { }

            return View(goods);
        }

        public ActionResult AddPurchase()
        {
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPurchase(Client client)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", client.PostId);
                return View(client);
            }

            var text = HttpContext.Request.Cookies["cart"].Value;
            if (text != null) text = HttpUtility.UrlDecode(text);
            List<GoodSettings> goods = new List<GoodSettings>();
            decimal price = 0;

            try
            {
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new Int32Converter());
                GoodInfo[] obj = JsonSerializer.Deserialize<GoodInfo[]>(text, serializeOptions);


                foreach (GoodInfo item in obj)
                {
                    Good Good = db.Goods.Find(item.goodId);
                    Size Size = db.Sizes.Find(item.sizeId);
                    Color Color = db.Colors.Find(item.colorId);
                    if (Color.GoodId != item.goodId) Color = null;

                    if (Good != null || Size != null || Color != null)
                    {
                        price += Good.Price;
                        goods.Add(new GoodSettings(Good, Size, Color));
                    }
                }
            }
            catch { }

            if (!(goods.Count > 0)) return RedirectToAction("Index", "Goods");

            db.Clients.Add(client);

            Purchase purch = new Purchase
            {
                Price = price,
                Date = DateTime.Now,
                ClientId = client.Id,
                PurchaseStateId = 1
            };

            db.Purchases.Add(purch);

            foreach (GoodSettings el in goods)
            {
                db.Carts.Add(new Cart
                {
                    PurchaseId = purch.Id,
                    GoodId = el.Good.Id,
                    SizeId = el.Size.Id,
                    ColorId = el.Color.Id,
                });
            }

            HttpContext.Response.Cookies["cart"].Value = HttpUtility.UrlEncode("[]");

            db.SaveChanges();

            return RedirectToAction("Index", "Goods");
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

    public class Int32Converter : System.Text.Json.Serialization.JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            throw new System.Text.Json.JsonException();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    class GoodInfo
    {
        public int goodId { get; set; }
        public int colorId { get; set; }
        public int sizeId { get; set; }
    }
}