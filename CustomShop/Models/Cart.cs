using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int GoodId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }

        public Color Color { get; set; }
        public Client Client { get; set; }
        public Good Good { get; set; }
        public Purchase Purchase { get; set; }
    }
}