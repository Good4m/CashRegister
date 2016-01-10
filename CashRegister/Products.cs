using CashRegister.Models;
using System.Collections.Generic;
using System;

namespace CashRegister
{
    public class Products : IActiveRecord<Product>
    {
        private Dictionary<string, Product> Items { get; set; }

        public Products()
        {
            Items = new Dictionary<string, Product>();
        }

        public Dictionary<string, Product> All()
        {
            return Items;
        }

        public void Create(string id, Product obj)
        {
            Items.Add(id, obj);
        }

        public Product Get(string id)
        {
            Product obj;
            Items.TryGetValue(id, out obj);
            return obj;
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
