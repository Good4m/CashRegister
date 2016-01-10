using CashRegister;
using CashRegister.DealConditions;
using CashRegister.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    /// Tests the register subsystem.
    /// </summary>
    [TestClass]
    public class RegisterTest
    {
        // Dummy product sold by unit
        Product cheerios = new Product(
            sku: "CHRO",
            name: "Box of Cheerios",
            regPrice: 6.99m,
            salePrice: 5.99m,
            chargeBy: Product.ChargeType.Unit,
            isOnSale: false
        );

        // Dummy product sold by weight
        Product apple = new Product(
            sku: "APPL",
            name: "Apple",
            regPrice: 2.49m,
            salePrice: 2.00m,
            chargeBy: Product.ChargeType.Weight,
            weight: 0.25m,
            isOnSale: false
        );

        /// <summary>
        ///  Tests that subtotal is correct when the product
        ///  is a type that is sold by quantity. 
        /// </summary>
        [TestMethod]
        public void CalculateSubtotalForProductSoldByUnit()
        {
            // Create subsystems
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            // Create product
            products.Create(cheerios.Sku, cheerios);

            // Add product to cart
            cart.AddProduct(cheerios);

            // Check if subtotal is correct after adding it
            decimal expected = cheerios.ActualPrice;
            decimal actual = register.CalculateSubtotal();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Tests that subtotal is correct when the product
        ///  is a type that is sold by weight. 
        /// </summary>
        [TestMethod]
        public void CalculateSubtotalForProductSoldByWeight()
        {
            // Create subsystems
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            // Create product
            products.Create(apple.Sku, apple);

            // Add product to cart
            cart.AddProduct(apple);

            // Check if subtotal is correct after adding it
            decimal expected = apple.ActualPrice * apple.Weight;
            decimal actual = register.CalculateSubtotal();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Tests if discounts are calculated correctly.
        /// </summary>
        [TestMethod]
        public void CalculateDiscountsTotal()
        {
            // Create subsystems
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            // Create product
            products.Create(cheerios.Sku, cheerios);

            // Add product to cart twice
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Create deal
            var cheeriosDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: cheerios.ActualPrice
            );

            // Add condition
            cheeriosDiscount.AddCondition(new QuantityCondition(
                product: cheerios,
                cart: cart,
                requiredQuantity: 2
            ));

            // Add deal
            deals.Create("Two for one Box of Cheerios.", cheeriosDiscount);

            // Calculate bill
            register.CalculateAllTotals();

            // Check if discount was calculated correctly
            decimal expected = cheeriosDiscount.DealValue;
            decimal actual = register.DiscountsTotal;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Tests if coupons are calculated correctly.
        /// </summary>
        [TestMethod]
        public void CalculateCouponsTotal()
        {
            // Create subsystems
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            // Create product
            products.Create(cheerios.Sku, cheerios);

            // Add product to cart twice
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Create deal
            // $1 off for every $10 spent
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1.00m
            );

            // Add condition
            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: register,
                amountRequired: 10.00m
            ));

            // Add deal
            deals.Create("$1 off for every $10 you spend.", oneOffForEveryTenCoupon);

            // Calculate bill
            register.CalculateAllTotals();

            // Check if coupon was calculated correctly
            decimal expected = (int)(register.SubTotal / 10m);
            decimal actual = register.CouponsTotal;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Tests if total amount due is calculated correctly.
        /// </summary>
        [TestMethod]
        public void CalculateTotalDue()
        {
            // Create subsystems
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            // Create product
            products.Create(cheerios.Sku, cheerios);

            // Add product
            cart.AddProduct(cheerios);

            // Calculate bill
            register.CalculateAllTotals();

            // Check if total due was calculated correctly
            decimal expected = register.SubTotal - register.DiscountsTotal - register.CouponsTotal + register.TaxesTotal;
            decimal actual = register.TotalDue;
            Assert.AreEqual(expected, actual);
        }


    }
}
