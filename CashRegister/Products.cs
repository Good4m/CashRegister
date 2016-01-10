using CashRegister.Models;
using System.Collections.Generic;
using System;

namespace CashRegister
{
    /// <summary>
    ///  Holds the products that are available to customers.
    /// </summary>
    public class Products : IActiveRecord<Product>
    {
        private Dictionary<string, Product> Items { get; set; }

        /// <summary>
        /// Initializes the collection.
        /// </summary>
        public Products()
        {
            Items = new Dictionary<string, Product>();
        }

        /// <inheritdoc />
        public Dictionary<string, Product> All()
        {
            return Items;
        }

        /// <inheritdoc />
        public void Create(string id, Product obj)
        {
            Items.Add(id, obj);
        }

        /// <inheritdoc />
        public Product Get(string id)
        {
            Product obj;
            Items.TryGetValue(id, out obj);
            return obj;
        }

        /// <inheritdoc />
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
