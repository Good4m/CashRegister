using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashRegister;
using CashRegister.Models;
using CashRegister.DealConditions;

namespace UnitTests
{
    /// <summary>
    /// Tests the deal system.
    /// </summary>
    [TestClass]
    public class DealTest
    {
        // Dummy product
        Product cheerios = new Product(
            sku: "CHRO",
            name: "Box of Cheerios",
            regPrice: 6.99m,
            salePrice: 5.99m,
            chargeBy: Product.ChargeType.Unit,
            isOnSale: false
        );

        /// <summary>
        ///  Creates and tests a discount: Two for One Box of Cheerios.
        ///  If we add two boxes to the cart, the expected value
        ///  should be the price of a single can.
        /// </summary>
        [TestMethod]
        public void CreateDealDiscount()
        {
            // Setup product subsystem
            Products products = new Products();
            products.Create(cheerios.Sku, cheerios);

            // Setup deal subsystem
            Deals deals = new Deals();

            // Setup cart subsystem
            Cart cart = new Cart();
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Setup register subsystem
            Register register = new Register(cart, products, deals);

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

            // Calculate totals
            register.CalculateAllTotals();

            // Check if the discount was calculated correctly
            decimal expected = cheerios.ActualPrice;
            decimal actual = register.SubTotal - register.DiscountsTotal;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Creates and tests a coupon:
        ///  $1 off for every $10 spent
        /// </summary>
        [TestMethod]
        public void CreateDealCoupon()
        {
            // Setup product subsystem
            Products products = new Products();
            products.Create(cheerios.Sku, cheerios);

            // Setup deals subsystem
            Deals deals = new Deals();

            // Setup cart subsystem
            Cart cart = new Cart();
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Setup register subsystem
            Register register = new Register(cart, products, deals);

            // Create coupon
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1
            );

            // Add condition
            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: register,
                amountRequired: 10
            ));

            // Check if correct amount is calculated to be taken off bill
            decimal expected = register.CalculateSubtotal() / 10;
            int actual   = oneOffForEveryTenCoupon.GetTimesApplyable();
            Assert.AreEqual((int)expected, actual);
        }
    }
}
