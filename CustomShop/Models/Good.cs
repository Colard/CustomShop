using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CustomShop.Models
{
    public class Good
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        public string Name { get; set; }

        [Display(Name = "Фотографія")]
        public byte[] Image { get; set; }

        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Ціна")]
        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        public decimal Price { get; set; }

        [Display(Name = "Тип товару")]
        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        public int GoodTypeId { get; set; }

        public GoodType GoodType { get; set; }
        
        public IEnumerable<Color> Colors { get; set; }
    }

    public class Color {
        public int Id { get; set; }
        [Display(Name = "Код кольору")]
        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        public string Code { get; set; }
        public int? GoodId { get; set; }

        public Good Good { get; set; }
    }

    public class GoodType {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}