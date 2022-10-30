using CustomShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CustomShop.Infrastructure;
using System.Security.Policy;
using Moq;
using Ninject;

namespace CustomShop.Controllers
{
    public class AdminController : Controller
    {
        IAuthProvider authProvider;
        public AdminController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        // GET: Admin
        public ActionResult Index()
        {   
            return View();
        }


        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password)) {
                    return Redirect(returnUrl ?? Url.Action("Index", "Purchases"));
                }

                ModelState.AddModelError("", "Неправильний логін або пароль");
                return View();
            }
         return View();
        }
    }
}