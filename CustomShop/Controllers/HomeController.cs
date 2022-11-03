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
using System.Data.Entity;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;

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

    class GoodInfo
    {
        public int goodId { get; set; }
        public int colorId { get; set; }
        public int sizeId { get; set; }
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

                    if (Good != null && Size != null && Color != null)
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

            string encryptedId = HttpUtility.UrlEncode(Encrypt(purch.Id.ToString()));
            return RedirectToAction("InfoAboutPurchase", "Home", new { id = encryptedId });
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult InfoAboutPurchase(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int purchaseID;

            try
            {
                purchaseID = Int32.Parse(Decrypt(HttpUtility.UrlDecode(id)));
            }
            catch
            {
                return HttpNotFound();
            }

            var purchase = db.Purchases.Find(purchaseID);
            purchase.Client = db.Clients.Find(purchase.ClientId);
            purchase.Client.Post = db.Posts.Find(purchase.Client.PostId);

            purchase.PurchaseState = db.PurchaseStates.Find(purchase.PurchaseStateId);

            purchase.Cart = db.Carts.Include(el => el.Good).Include(el => el.Size).Include(el => el.Color).Where(el => el.PurchaseId == purchase.Id);
            return View(purchase);
        }

        private static string Encrypt(string ishText, string pass = "gno34uhuGSsfhe4sa",
        string sol = "asddsa", string cryptographicAlgorithm = "SHA1",
        int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
         int keySize = 256)
        {
            if (string.IsNullOrEmpty(ishText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string ciphText, string pass = "gno34uhuGSsfhe4sa",
               string sol = "asddsa", string cryptographicAlgorithm = "SHA1",
               int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(ciphText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
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

}