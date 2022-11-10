using CustomShop.Controllers;
using CustomShop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace CustomShopTests.ControllersNames
{
    [TestClass]
    public class PurchasesControllerTests
    {
        [TestMethod]
        public void IndexReturnNotNull()
        {
            // Arrange
            PurchasesController controller = new PurchasesController();

            // Act
            ViewResult result = controller.Index(null, "txt", null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditReturnObjectID()
        {
            // Arrange
            PurchasesController controller = new PurchasesController();

            // Act
            ViewResult result = controller.Edit(4) as ViewResult;

            // Assert
            Assert.IsTrue((result.Model as Purchase).Id == 4);
        }
    }
}
