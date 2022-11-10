using CustomShop.Controllers;
using CustomShop.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace CustomShopTests.ControllersNames
{
    [TestClass]
    public class AdminControllerTests
    {
        [TestMethod]
        public void AdminControllerAutorizeIsNotNull()
        {
            AdminController controller = new AdminController(new FormAuthProvider());

            ActionResult resault = controller.Index();

            Assert.IsNotNull(resault);
        }
    }
}
