using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashRegister;
using CashRegister.Models;
using CashRegister.DealConditions;

namespace UnitTests
{
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

        [TestMethod]
        public void CreateDealDiscount()
        {
            Products products = new Products();
            products.Create(cheerios.Sku, cheerios);
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Create deal
            var cheeriosDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: cheerios.ActualPrice
            );
            cheeriosDiscount.AddCondition(new QuantityCondition(
                product: cheerios,
                cart: cart,
                requiredQuantity: 2
            ));
            deals.Create("Two for one Box of Cheerios.", cheeriosDiscount);

            register.CalculateAllTotals();

            decimal expected = cheerios.ActualPrice;
            decimal actual = register.SubTotal - register.DiscountsTotal;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateDealCoupon()
        {
            Products products = new Products();
            products.Create(cheerios.Sku, cheerios);
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // Create coupon
            // $1 off for every $10 spent
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1
            );
            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: register,
                amountRequired: 10
            ));

            decimal subtotal = register.CalculateSubtotal();

            decimal expected = subtotal / 10;
            int actual   = oneOffForEveryTenCoupon.GetTimesApplyable();
            Assert.AreEqual((int)expected, actual);
        }
    }
}
