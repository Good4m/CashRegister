using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashRegister;
using CashRegister.Models;

namespace UnitTests
{
    /// <summary>
    ///  Tests the shopping cart system.
    /// </summary>
    [TestClass]
    public class CartTest
    {
        /// <summary>
        ///  Tests if a product can be added to the cart.
        /// </summary>
        [TestMethod]
        public void AddProduct()
        {
            Cart cart = new Cart();

            // Create a product
            Product testProduct = new Product(
                sku: "TEST",
                name: "TEST PRODUCT",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.15m
            );

            // Add product to cart
            cart.AddProduct(testProduct);

            // Check if it was added successfully
            bool expected = true;
            bool actual   = cart.Products.ContainsKey(testProduct.Sku);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Tests if a product can be removed from the cart.
        /// </summary>
        [TestMethod]
        public void RemoveProduct()
        {
            Cart cart = new Cart();

            // Create a product
            Product testProduct = new Product(
                sku: "TEST",
                name: "TEST PRODUCT",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.15m
            );

            // Add product to cart
            cart.AddProduct(testProduct);

            // Remove product from cart
            cart.RemoveProduct(testProduct);

            // Check if it was removed successfully
            bool expected = false;
            bool actual = cart.Products.ContainsKey(testProduct.Sku);
            Assert.AreEqual(expected, actual);
        }
    }
}
