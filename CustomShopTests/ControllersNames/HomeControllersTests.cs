using CustomShop.Controllers;
using CustomShop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace CustomShopTests.ControllersNames
{
    [TestClass]
    public class HomeControllersTests
    {
        [TestMethod]
        public void IndexFilterReturnNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddPurchaseReturnIsNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.AddPurchase() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InfoAboutPurchaseReturnSecureIsTrue()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            HttpNotFoundResult result = controller.InfoAboutPurchase("12") as HttpNotFoundResult;

            // Assert
            Assert.IsTrue(result != null);
        }
    }
}

