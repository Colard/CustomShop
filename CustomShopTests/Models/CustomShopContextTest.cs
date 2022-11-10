using CustomShop.Controllers;
using CustomShop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace CustomShopTests.Models
{
    [TestClass]
    public class CustomShopContextTest
    {
        [TestMethod]
        public void ContextReturnNotNull()
        {
            ShopContext db = new ShopContext();

            // Assert
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void ModelReturnNotNull()
        {
            ShopContext db = new ShopContext();
            // Arrange


            // Act
            Purchase model = db.Purchases.Find(4);

            // Assert
            Assert.IsTrue((model as Purchase).Id == 4);
        }
    }
}
