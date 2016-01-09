using CashRegister.Models;
using System.Collections;
using System.Collections.Generic;

namespace CashRegister
{
    class Cart
    {
        public Dictionary<string, int> Products { get; set; } = new Dictionary<string, int>();

        public void AddProduct(Product p)
        {
            int currentQuantity = 0;
            Products.TryGetValue(p.ProdCode, out currentQuantity);
            Products.Add(p.Name, ++currentQuantity);
        }

        public void RemoveProduct(Product p)
        {
            int currentQuantity = 0;
            if (Products.TryGetValue(p.ProdCode, out currentQuantity))
            {
                if (currentQuantity > 1)
                    Products[p.ProdCode]--;
                else
                    Products.Remove(p.ProdCode);
            }
        }

    }
}
