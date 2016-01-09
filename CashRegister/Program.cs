using CashRegister.Models;
using System;
using System.Collections.Generic;

namespace CashRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Jeff's Cash Register.");
            Console.WriteLine();
            Console.WriteLine("Please select which products to add to your cart:");

            DisplayProductList();

            Cart cart = new Cart();

            while (true)
            {
                string productCode = AskUserForProductCode();

                if (productCode == "checkout")
                    break;

                Product prod = Product.Get(productCode);

                if (prod != null)
                {
                    cart.AddProduct(prod);

                    Console.WriteLine("Product added to cart. "
                        + "You now have " + cart.Products.Count + " item(s).");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }

            Register register = new Register();
            register.Checkout(cart);

            //if (register.Discount > 0)
            //    Console.WriteLine("Congratulations, you saved " + FormatAsMoney(register.Discount) + " in discounts today!");

            // DO YOU HAVE ANY COUPONS?
            Console.WriteLine();
            Console.WriteLine("You are eligible for the following coupons:");
            Console.WriteLine("2 for 1 apples");
            Console.WriteLine("2 for 1 apples");
            Console.WriteLine("2 for 1 apples");
            Console.WriteLine("Would you like to apply them now?");

            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                // APPLY COUPONS HERE TO CHANGE BILL
            }


            Console.WriteLine(" ");
            Console.WriteLine("Your bill:");
            Console.WriteLine("----------------");
            Console.WriteLine("Sub-total: " + FormatAsMoney(register.SubTotal));
            Console.WriteLine("Taxes:     " + FormatAsMoney(register.Taxes));
            Console.WriteLine("Discount:  " + FormatAsMoney(register.Discount));
            Console.WriteLine();
            Console.WriteLine("Total:     " + FormatAsMoney(register.Total));
            Console.WriteLine("----------------");
            Console.WriteLine();

            
            Console.WriteLine("Thank you for shopping. Press any key to leave the store.");
            Console.ReadLine();
        }

        private static string FormatAsMoney(decimal dec)
        {
            return string.Format("{0:C}", dec);
        }

        private static void DisplayProductList()
        {
            Console.WriteLine();
            Console.WriteLine("Code\tPrice\t\tName");
            Console.WriteLine("-------------------------------");

            foreach (KeyValuePair<string, Product> p in Product.All)
            {
                string chargeby = "";

                if (p.Value.ChargeBy == Product.ChargeType.Unit)
                    chargeby = " each ";

                if (p.Value.ChargeBy == Product.ChargeType.Weight)
                    chargeby = " per lb. ";


                Console.Write(p.Key + "\t");
                if (p.Value.IsOnSale)
                {
                    Console.Write("$" + p.Value.SalePrice + chargeby + "(ON SALE!)");
                }
                else
                {
                    Console.Write("$" + p.Value.RegPrice + chargeby);
                }
                Console.Write("\t" + p.Value.Name);
                Console.WriteLine();
            }
        }

        private static string AskUserForProductCode()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the code of the product you wish to add, or type 'checkout' to pay:");
            Console.Write("> ");
            string input = Console.ReadLine();

            for (int i = 0; i < 16; i++)
            {
                switch (i % 4)
                {
                    case 0: Console.Write("/");  break;
                    case 1: Console.Write("-");  break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|");  break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                System.Threading.Thread.Sleep(50);
            }
            return input;
        }
    }
}
