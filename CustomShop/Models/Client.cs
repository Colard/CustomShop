using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CustomShop.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Ім`я")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Ім`я не має перевищувати 30 символів")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Прізвище не має перевищувати 30 символів")]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "По батькові не має перевищувати 30 символів")]
        [Display(Name = "По батькові")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Номер телефону")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Телефон може мати від 8 до 20 символів")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Ваша електронна пошта")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некоректна адреса")]
        public string E_Mail { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Адреса")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поштовий індекс України має лише 5 символів")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Некоректний індекс")]
        public string PostIndex { get; set; }

        [Required(ErrorMessage = "Поле повинно бути встановлено")]
        [Display(Name = "Пошта")]
        public int PostId { get; set; }

        public Post Post { get; set; }
    }

    public class Purchase
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int PurchaseStateId { get; set; }

        public PurchaseState PurchaseState { get; set; }
        public Client Client { get; set; }
        public IEnumerable<Cart> Cart { get; set; }

    }

    public class PurchaseState
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}