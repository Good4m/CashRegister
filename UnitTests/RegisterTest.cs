using CashRegister;
using CashRegister.DealConditions;
using CashRegister.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
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

        [TestMethod]
        public void CalculateSubtotalForProductSoldByUnit()
        {
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            products.Create(cheerios.Sku, cheerios);
            cart.AddProduct(cheerios);

            decimal expected = cheerios.ActualPrice;
            decimal actual = register.CalculateSubtotal();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateSubtotalForProductSoldByWeight()
        {
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            products.Create(apple.Sku, apple);
            cart.AddProduct(apple);

            decimal expected = apple.ActualPrice * apple.Weight;
            decimal actual = register.CalculateSubtotal();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateDiscountsTotal()
        {
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            products.Create(cheerios.Sku, cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

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

            decimal expected = cheeriosDiscount.DealValue;
            decimal actual = register.DiscountsTotal;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateCouponsTotal()
        {
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            products.Create(cheerios.Sku, cheerios);
            cart.AddProduct(cheerios);
            cart.AddProduct(cheerios);

            // $1 off for every $10 spent
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1.00m
            );

            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: register,
                amountRequired: 10.00m
            ));

            deals.Create("$1 off for every $10 you spend.", oneOffForEveryTenCoupon);

            register.CalculateAllTotals();

            decimal expected = (int)(register.SubTotal / 10m);
            decimal actual = register.CouponsTotal;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void CalculateTotalDue()
        {
            Products products = new Products();
            Deals deals = new Deals();
            Cart cart = new Cart();
            Register register = new Register(cart, products, deals);

            products.Create(cheerios.Sku, cheerios);
            cart.AddProduct(cheerios);

            register.CalculateAllTotals();

            decimal expected = register.SubTotal - register.DiscountsTotal - register.CouponsTotal + register.TaxesTotal;
            decimal actual = register.TotalDue;
            Assert.AreEqual(expected, actual);
        }


    }
}
