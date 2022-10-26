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

        public Good Good { get; set; }
        public Purchase Purchase { get; set; }
    }
}