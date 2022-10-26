using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CustomShop.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
    }
}