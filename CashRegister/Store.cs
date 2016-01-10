using CashRegister.DealConditions;
using CashRegister.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister
{
    /// <summary>
    ///  Represents the store by using the facade pattern.
    /// </summary>
    public class Store
    {
        /// <summary>
        /// Manages a collection of products.
        /// </summary>
        private Products _products;

        /// <summary>
        /// Manages a collection of deals.
        /// </summary>
        private Deals _deals;

        /// <summary>
        /// Manages the items in the customer's shopping cart.
        /// </summary>
        private Cart _cart;

        /// <summary>
        /// Manages calculations.
        /// </summary>
        private Register _register;

        /// <summary>
        /// Initializes the store.
        /// </summary>
        public Store()
        {
            // Initialize subsystems
            _products = new Products();
            _deals = new Deals();
            _cart = new Cart();
            _register = new Register(_cart, _products, _deals);

            // Create some seed data to work with
            CreateDummyProducts();
            CreateDummyDeals();

            // Welcome to user and provide instructions
            Console.WriteLine("Welcome to Jeff's Store.");

            // Print commands
            DisplayCommandList();

            Console.WriteLine();

            // Print products
            DisplayProductList();

            Console.WriteLine();
            Console.WriteLine("\nPlease select which products to add to your cart:");

            // Continuously get input from user via commands
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter the code of the product you wish to add, or type 'help' for a list of commands:");

                // Get input from console
                string userInput = GetUserInput();

                // Products command
                if (userInput == "products")
                {
                    DisplayProductList();
                    continue;
                }

                // Deals command
                if (userInput == "deals")
                {
                    DisplayDealList();
                    continue;
                }

                // Cart command
                if (userInput == "cart")
                {
                    DisplayCart();
                    continue;
                }

                // Checkout command
                if (userInput == "checkout")
                    break;

                // Help command
                if (userInput == "help")
                    DisplayCommandList();

                // Leave command
                if (userInput == "leave")
                    return;

                // Show user some feedback
                ShowSpinner(8);

                // If we reach this point, we know the user
                // types a product code instead of a command,
                // so we attempt to find the product.
                Product prod = _products.Get(userInput.ToUpper());

                // Product found
                if (prod != null)
                {
                    // Add product to cart
                    _cart.AddProduct(prod);

                    Console.WriteLine("  Product added to cart. "
                        + "You now have " + _cart.Size() + " item(s).");
                }
                // Product not found
                else
                {
                    Console.WriteLine("  Product not found.");
                }
            }

            // Reaching this point means the user has chosen to checkout or leave

            // Calculate the customer's bill
            _register.CalculateAllTotals();

            ShowSpinner(16);

            // Display cart contents
            DisplayCart();

            ShowSpinner(16);

            // Display deals applied to bill
            DisplayDealsApplied();

            ShowSpinner(16);

            // Display bill and thank you to customer.
            DisplayBill();
        }

        /// <summary>
        ///  Prints out a summary of the customer's bill.
        /// </summary>
        private void DisplayBill()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Your bill:");
            Console.WriteLine("----------------------");
            ShowSpinner(16);
            Console.WriteLine("Sub-total:      " + Utilities.FormatAsMoney(_register.SubTotal));
            Console.WriteLine("Discounts:     -" + Utilities.FormatAsMoney(_register.DiscountsTotal));
            Console.WriteLine("Coupons:       -" + Utilities.FormatAsMoney(_register.CouponsTotal));
            Console.WriteLine("Taxes:          " + Utilities.FormatAsMoney(_register.TaxesTotal));
            Console.WriteLine();
            Console.WriteLine("Total:          " + Utilities.FormatAsMoney(_register.TotalDue));
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.WriteLine("Thank you for shopping. Press <enter> to leave the store.");
            Console.ReadLine(); // Prevents console from closing
        }

        /// <summary>
        ///  Prints a summary of deals applied to the bill.
        /// </summary>
        private void DisplayDealsApplied()
        {
            StringBuilder discountStr = new StringBuilder();
            StringBuilder couponStr = new StringBuilder();

            int validDiscounts = 0;
            int validCoupons = 0;

            // Loops through all deals and builds summaries for each deal type
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
            Console.WriteLine();

            // Prints out all available deals
            if (validDiscounts > 0 || validCoupons > 0)
            {
                Console.WriteLine("You are eligible for the following deal(s):");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine(discountStr);
                Console.WriteLine(couponStr);
                Console.WriteLine("You saved " + Utilities.FormatAsMoney(_register.CouponsTotal + _register.DiscountsTotal) + "!");
            }
            // No deals available
            else
            {
                Console.WriteLine("You are not eligible for any deals.");
                Console.WriteLine();
            }
        }

        /// <summary>
        ///  Displays a summary of available commands.
        /// </summary>
        private void DisplayCommandList()
        {
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine();
            Console.WriteLine("Command      Description");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("> products   Displays available products.");
            Console.WriteLine("> deals      Displays discounts and coupons flyer.");
            Console.WriteLine("> cart       Displays items in your cart.");
            Console.WriteLine("> checkout   Finish shopping and pay.");
            Console.WriteLine("> leave      Leave the store.");
        }

        /// <summary>
        ///  Displays a summary of the customer's shopping cart.
        /// </summary>
        private void DisplayCart()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Cart:");
            Console.WriteLine();
            Console.WriteLine("Qty\tPrice\t\tName");
            Console.WriteLine("---------------------------------------");

            // Iterate over each product in the customer's cart
            foreach (KeyValuePair<string, int> entry in _cart.Products)
            {
                Product product = _products.Get(entry.Key);
                // How is this product charged? By unit or by lb.?
                string chargeby = "";
                if (product.ChargeBy == Product.ChargeType.Unit)
                {
                    chargeby = " each ";
                }
                else if (product.ChargeBy == Product.ChargeType.Weight)
                {
                    chargeby = " per lb. ";
                }

                Console.WriteLine(entry.Value + "x\t" + Utilities.FormatAsMoney(product.ActualPrice)
                    + chargeby + "\t" + product.Name);
            }
        }

        /// <summary>
        ///  Displays a summary of available products.
        /// </summary>
        private void DisplayProductList()
        {
            Console.WriteLine("Product list:");
            Console.WriteLine();
            Console.WriteLine("Code\tPrice\t\tName");
            Console.WriteLine("---------------------------------------");

            // Iterate over every available product
            foreach (KeyValuePair<string, Product> entry in _products.All())
            {
                string productCode = entry.Key;
                Product product = entry.Value;

                // How is this product charged? By unit or by lb.?
                string chargeby = "";
                if (product.ChargeBy == Product.ChargeType.Unit)
                {
                    chargeby = " each ";
                }
                else if (product.ChargeBy == Product.ChargeType.Weight)
                {
                    chargeby = " per lb. ";
                }

                // Print product code
                Console.Write(productCode + "\t");

                // Print price
                Console.Write("$" + product.ActualPrice + chargeby);

                // Print whether the product is on sale or not
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

        /// <summary>
        ///  Displays a summary of the deals available.
        /// </summary>
        private void DisplayDealList()
        {
            Console.WriteLine();
            Console.WriteLine("Available Deals:");
            Console.WriteLine("---------------------------------------");

            StringBuilder discounts = new StringBuilder();
            StringBuilder coupons = new StringBuilder();

            // Prepares lists for each deal type available
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

            // Print out all deals
            Console.WriteLine("Discounts:");
            Console.WriteLine(discounts);
            Console.WriteLine("Coupons:");
            Console.WriteLine(coupons);
        }

        /// <summary>
        ///  Gets console input from the user.
        /// </summary>
        /// <returns></returns>
        private string GetUserInput()
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>
        ///  Shows an animated spinner to imply that work is being done.
        ///  It makes the console a little more readable when pushing
        ///  lots of information to it at once.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="message"></param>
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

        /// <summary>
        /// Creates some dummy products to use.
        /// </summary>
        private void CreateDummyProducts()
        {
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

            var groundChickenHalf = new Product(
                sku: "CHKN1",
                name: "Ground Chicken 1/2 lb.",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 0.5m
            );

            var groundChickenDouble = new Product(
                sku: "CHKN2",
                name: "Ground Chicken 2 lb.",
                regPrice: 4.99m,
                salePrice: 4.65m,
                chargeBy: Product.ChargeType.Weight,
                weight: 2m
            );

            // Add products
            _products.Create(cheerios.Sku, cheerios);
            _products.Create(redbull.Sku, redbull);
            _products.Create(apple.Sku, apple);
            _products.Create(groundChickenHalf.Sku, groundChickenHalf);
            _products.Create(groundChickenDouble.Sku, groundChickenDouble);
        }

        /// <summary>
        ///  Creates some dummy deals to use.
        /// </summary>
        private void CreateDummyDeals()
        {
            // Create 2 for 1 Box of Cheerios discount
            var cheerios = _products.Get("CHRO");

            // Create deal
            var cheeriosDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: cheerios.ActualPrice
            );

            // Add condition
            cheeriosDiscount.AddCondition(new QuantityCondition(
                product: cheerios,
                cart: _cart,
                requiredQuantity: 2
            ));

            // Add to collection
            _deals.Create("Two for one Box of Cheerios.", cheeriosDiscount);


            // Create 4 Redbulls and get one free discount
            var redbull = _products.Get("RDBL");

            // Create deal
            var redbullDiscount = new Deal(
                type: Deal.DealType.Discount,
                value: redbull.ActualPrice
            );

            // Add condition
            redbullDiscount.AddCondition(new QuantityCondition(
                product: redbull,
                cart: _cart,
                requiredQuantity: 5
            ));

            // Add to collection
            _deals.Create("Buy four Redbulls and get one free.", redbullDiscount);


            // Create $1 off for every $10 spent coupon
            var oneOffForEveryTenCoupon = new Deal(
                type: Deal.DealType.Coupon,
                value: 1.00m
            );

            // Add condition
            oneOffForEveryTenCoupon.AddCondition(new CartTotalCondition(
                register: _register,
                amountRequired: 10.00m
            ));

            // Add to collection
            _deals.Create("$1 off for every $10 you spend.", oneOffForEveryTenCoupon);


            // Create $1 off for every $20 spent coupon
            // if cart includes at least two Boxes of Cheerios
            var complexDeal = new Deal(
                type: Deal.DealType.Coupon,
                value: 1.00m
            );

            // Add condition
            complexDeal.AddCondition(new CartTotalCondition(
                register: _register,
                amountRequired: 20.00m
            ));

            // Add another condition
            complexDeal.AddCondition(new QuantityCondition(
                product: redbull,
                cart: _cart,
                requiredQuantity: 2
            ));

            // Add to collection
            _deals.Create("$1 off for every $20 you spend if you buy at least two Cans of Redbull.", complexDeal);
        }
    }
}
