using CustomShop.Controllers;
using CustomShop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace CustomShopTests.ControllersNames
{
    [TestClass]
    public class GoodsEditorControllerTest
    {
        [TestMethod]
        public void IndexFilterReturnNotNull()
        {
            // Arrange
            GoodsEditorController controller = new GoodsEditorController();

            // Act
            ViewResult result = controller.Index("фут", 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DetailsReturnObjectID()
        {
            // Arrange
            GoodsEditorController controller = new GoodsEditorController();

            // Act
            ViewResult result = controller.Details(4) as ViewResult;

            // Assert
            Assert.IsTrue((result.Model as Good).Id == 4);
        }

        [TestMethod]
        public void CreateReturnIsNotNull()
        {
            // Arrange
            GoodsEditorController controller = new GoodsEditorController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditReturnIsNotNull()
        {
            // Arrange
            GoodsEditorController controller = new GoodsEditorController();

            // Act
            ViewResult result = controller.Edit(4) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
