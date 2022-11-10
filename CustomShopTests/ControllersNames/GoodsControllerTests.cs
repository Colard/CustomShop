using CustomShop.Controllers;
using CustomShop.Models;
using CustomShop.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using Microsoft.CSharp;

namespace CustomShopTests.ControllersNames
{
    [TestClass]
    public class GoodsControllerTests
    {
        [TestMethod]
        public void IndexViewBagReturnNotNull()
        {
            // Arrange
            GoodsController controller = new GoodsController();
            
            // Act
            ViewResult result = controller.Index("Усі товари") as ViewResult;

            // Assert
            Assert.IsNotNull(result.ViewBag.Filter);
        }

        [TestMethod]
        public void DetailsReturnObjectID()
        {
            // Arrange
            GoodsController controller = new GoodsController();

            // Act
            ViewResult result = controller.Details(4) as ViewResult;

            // Assert
            Assert.IsTrue((result.Model as Good).Id == 4);
        }

        [TestMethod]
        public void DetailsReturnIsNotNull()
        {
            // Arrange
            GoodsController controller = new GoodsController();

            // Act
            ViewResult result = controller.Details(4) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
