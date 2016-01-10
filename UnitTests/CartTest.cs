using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashRegister;
using CashRegister.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void AddProduct()
        {
            Cart cart = new Cart();

            Product testProduct = new Product(
                sku: "TEST",
                name: "TEST PRODUCT",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.15m
            );

            cart.AddProduct(testProduct);

            bool expected = true;
            bool actual   = cart.Products.ContainsKey(testProduct.Sku);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveProduct()
        {
            Cart cart = new Cart();

            Product testProduct = new Product(
                sku: "TEST",
                name: "TEST PRODUCT",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.15m
            );

            cart.RemoveProduct(testProduct);

            bool expected = false;
            bool actual = cart.Products.ContainsKey(testProduct.Sku);
            Assert.AreEqual(expected, actual);
        }
    }
}
