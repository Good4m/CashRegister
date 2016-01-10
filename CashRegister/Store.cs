using CashRegister.DealConditions;
using CashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    public class Store
    {
        private Products _products;
        private Deals _deals;
        private Cart _cart;
        private Register _register;

        public Store()
        {
            _products = new Products();
            _deals = new Deals();
            _cart = new Cart();
            _register = new Register(_cart, _products, _deals);


            CreateDummyProducts();
            CreateDummyDeals();

            Console.WriteLine("Welcome to Jeff's Store.");
            Console.WriteLine();
            DisplayCommandList();
            Console.WriteLine();
            //DisplayDealList();
            DisplayProductList();

            Console.WriteLine();
            Console.WriteLine("Please select which products to add to your cart:");

            while (true)
            {
                string userInput = AskUserForProductCode();

                if (userInput == "products")
                {
                    DisplayProductList();
                    continue;
                }

                if (userInput == "deals")
                {
                    DisplayDealList();
                    continue;
                }

                if (userInput == "cart")
                {
                    DisplayCart();
                    continue;
                }

                if (userInput == "checkout")
                    break;

                if (userInput == "leave")
                    return;

                ShowSpinner(8);

                Product prod = _products.Get(userInput);

                if (prod != null)
                {
                    _cart.AddProduct(prod);

                    Console.WriteLine("  Product added to cart. "
                        + "You now have " + _cart.Size() + " item(s).");
                }
                else
                {
                    Console.WriteLine("  Product not found.");
                }
            }

            _register.CalculateAllTotals();


            // Display deals claimed
            StringBuilder discountStr = new StringBuilder();
            StringBuilder couponStr = new StringBuilder();

            int validDiscounts = 0;
            int validCoupons = 0;

            foreach (KeyValuePair<string, Deal> entry in _deals.All())
            {
                int timesSatisfied = entry.Value.GetTimesApplyable();
                if (timesSatisfied > 0)
                {
                    discountStr.Append(entry.Value.Type + ": " + timesSatisfied + "x - " + entry.Key + "\n");

                    if (entry.Value.Type == Deal.DealType.Discount)
                    {
                        validDiscounts++;
                    }
                    else if (entry.Value.Type == Deal.DealType.Coupon)
                    {
                        validCoupons++;
                    }
                }
            }

            Console.WriteLine(" ");

            if (validDiscounts > 0 || validCoupons > 0)
            {
                Console.WriteLine("You are eligible for the following deal(s):");
                ShowSpinner(16);
                Console.WriteLine(discountStr);
                Console.WriteLine(couponStr);
                Console.WriteLine("You saved " + Utilities.FormatAsMoney(_register.CouponsTotal + _register.DiscountsTotal) + "!");
            }
            else
            {
                Console.WriteLine("You are not eligible for any deals.");
            }

            Console.WriteLine(" ");
            Console.WriteLine("ºYour bill:");
            Console.WriteLine("---------------------");
            ShowSpinner(16, "Calculating...");
            Console.WriteLine("Sub-total:      " + Utilities.FormatAsMoney(_register.SubTotal));
            Console.WriteLine("Discounts:     -" + Utilities.FormatAsMoney(_register.DiscountsTotal));
            Console.WriteLine("Coupons:       -" + Utilities.FormatAsMoney(_register.CouponsTotal));
            Console.WriteLine("Taxes:          " + Utilities.FormatAsMoney(_register.TaxesTotal));
            Console.WriteLine();
            Console.WriteLine("Total:          " + Utilities.FormatAsMoney(_register.TotalDue));
            Console.WriteLine("---------------------");
            Console.WriteLine();


            Console.WriteLine("Thank you for shopping. Press any key to leave the store.");
            Console.ReadLine();
            Console.ReadLine();
        }

        private void DisplayCommandList()
        {
            Console.WriteLine("ºCommands:");
            Console.WriteLine();
            Console.WriteLine("Command      Description");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("> products   Displays available products.");
            Console.WriteLine("> deals      Displays discounts and coupons flyer.");
            Console.WriteLine("> cart       Displays items in your cart.");
            Console.WriteLine("> checkout   Finish shopping and pay.");
            Console.WriteLine("> leave      Leave the store.");
        }

        private void DisplayCart()
        {
            Console.WriteLine("ºCart:");
            Console.WriteLine();
            Console.WriteLine("Qty\tPrice\t\tName");
            Console.WriteLine("---------------------------------------");

            foreach (KeyValuePair<string, int> entry in _cart.Products)
            {
                Console.WriteLine(entry.Value + "x\t" + Utilities.FormatAsMoney(_products.Get(entry.Key).ActualPrice) + "\t\t" + _products.Get(entry.Key).Name);
            }
        }

        private void DisplayProductList()
        {
            Console.WriteLine();
            Console.WriteLine("ºProduct list:");
            Console.WriteLine();
            Console.WriteLine("Code\tPrice\t\tName");
            Console.WriteLine("---------------------------------------");

            foreach (KeyValuePair<string, Product> entry in _products.All())
            {
                string chargeby = "";
                Product product = entry.Value;

                if (product.ChargeBy == Product.ChargeType.Unit)
                    chargeby = " each ";

                if (product.ChargeBy == Product.ChargeType.Weight)
                    chargeby = " per lb. ";


                Console.Write(entry.Key + "\t");
                Console.Write("$" + product.ActualPrice + chargeby);
                if (product.IsOnSale)
                {
                    Console.Write("\t" + product.Name + " (**ON SALE** Previously " + Utilities.FormatAsMoney(product.RegPrice) + ")");
                }
                else
                {
                    Console.Write("\t" + product.Name);
                }

                Console.WriteLine();
            }
        }

        private void DisplayDealList()
        {
            Console.WriteLine();
            Console.WriteLine("ºAvailable Deals:");
            Console.WriteLine("---------------------------------------");
            StringBuilder discounts = new StringBuilder();
            StringBuilder coupons = new StringBuilder();
            foreach (KeyValuePair<string, Deal> entry in _deals.All())
            {
                Deal.DealType type = entry.Value.Type;
                if (entry.Value.Type == Deal.DealType.Discount)
                {
                    discounts.Append("- " + entry.Key + "\n");
                }
                else if (type == Deal.DealType.Coupon)
                {
                    coupons.Append("- " + entry.Key + "\n");
                }
            }
            Console.WriteLine("Discounts:");
            Console.WriteLine(discounts);
            Console.WriteLine("Coupons:");
            Console.WriteLine(coupons);

        }


        private string AskUserForProductCode()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the code of the product you wish to add, or type 'help' for a list of commands:");
            Console.Write("> ");
            string input = Console.ReadLine();
            return input;
        }

        private void ShowSpinner(int length, string message = "")
        {
            for (int i = 0; i < length; i++)
            {
                switch (i % 4)
                {
                    case 0: Console.Write("/ " + message); break;
                    case 1: Console.Write("- " + message); break;
                    case 2: Console.Write("\\ " + message); break;
                    case 3: Console.Write("| " + message); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 2 - message.Length, Console.CursorTop);
                System.Threading.Thread.Sleep(50);
            }
        }


        private void CreateDummyProducts()
        {
            // Create dummy products
            var cheerios = new Product(
                sku: "CHRO",
                name: "Box of Cheerios",
                regPrice: 6.99m,
                salePrice: 5.99m,
                chargeBy: Product.ChargeType.Unit
            );

            var redbull = new Product(
                sku: "RDBL",
                name: "Can of Redbull",
                regPrice: 3.99m,
                salePrice: 2.50m,
                chargeBy: Product.ChargeType.Unit
            );

            var apple = new Product(
                sku: "APPL",
                name: "Apple",
                regPrice: 2.49m,
                salePrice: 2.00m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.25m,
                isOnSale: true
            );

            var groundbeef = new Product(
                sku: "GRDB",
                name: "Ground Beef",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.15m
            );

            _products.Create(cheerios.Sku, cheerios);
            _products.Create(redbull.Sku, redbull);
            _products.Create(apple.Sku, apple);
            _products.Create(groundbeef.Sku, groundbeef);
        }

        private void CreateDummyDeals()
        {
            // 2 for 1 Box of Cheerios
            var cheerios = _products.Get("CHRO");
            var cheeriosDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: cheerios.ActualPrice
            );
            cheeriosDiscount.AddCondition(new QuantityCondition(
                product: cheerios,
                cart: _cart,
                requiredQuantity: 2
            ));
            _deals.Create("Two for one Box of Cheerios.", cheeriosDiscount);

            // Buy 4 Redbulls and get one free
            var redbull = _products.Get("RDBL");
            var redbullDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: redbull.ActualPrice
            );
            redbullDiscount.AddCondition(new QuantityCondition(
                product: redbull,
                cart: _cart,
                requiredQuantity: 5
            ));
            _deals.Create("Buy four Redbulls and get one free.", redbullDiscount);

            // $1 off for every $10 spent
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1.00m
            );
            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: _register,
                amountRequired: 10.00m
            ));
            _deals.Create("$1 off for every $10 you spend.", oneOffForEveryTenCoupon);
        }
    }
}
