using CashRegister.Models;
using System.Collections;
using System.Collections.Generic;

namespace CashRegister
{
    /// <summary>
    /// Represents the customer's shopping cart.
    /// </summary>
    public class Cart
    {
        /// <summary>
        ///  Holds products in the cart.
        /// </summary>
        public Dictionary<string, int> Products { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Adds a product to the cart.
        /// </summary>
        /// <param name="p"></param>
        public void AddProduct(Product p)
        {
            int currentQuantity = 0;
            if (Products.TryGetValue(p.Sku, out currentQuantity))
            {
                Products[p.Sku] = currentQuantity + 1;
            } else
            {
                Products.Add(p.Sku, 1);
            }
        }

        /// <summary>
        ///  Removes a product from the cart.
        /// </summary>
        /// <param name="p"></param>
        public void RemoveProduct(Product p)
        {
            int currentQuantity = 0;
            if (Products.TryGetValue(p.Sku, out currentQuantity))
            {
                if (currentQuantity > 1)
                    Products[p.Sku]--;
                else
                    Products.Remove(p.Sku);
            }
        }

        /// <summary>
        ///  Returns the number of products in the cart.
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            int count = 0;
            foreach (KeyValuePair<string, int> entry in Products)
            {
                count += entry.Value;
            }
            return count;
        }

    }
}
