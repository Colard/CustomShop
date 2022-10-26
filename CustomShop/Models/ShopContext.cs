using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CustomShop.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext() 
            : base("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\DataBase.mdf;Integrated Security=True") {}

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseState> PurchaseStates { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<GoodType> GoodTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
    }
}