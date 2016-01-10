using System;
using System.Collections.Generic;

namespace CashRegister
{
    /// <summary>
    ///  Holds the deals that are available to customers.
    /// </summary>
    public class Deals : IActiveRecord<Deal>
    {
        /// <summary>
        ///  Collection that holds all deals.
        /// </summary>
        private Dictionary<string, Deal> Items { get; set; }

        /// <summary>
        ///  Initialized the collection.
        /// </summary>
        public Deals()
        {
            Items = new Dictionary<string, Deal>();
        }

        /// <inheritdoc />
        public Dictionary<string, Deal> All()
        {
            return Items;
        }

        /// <inheritdoc />
        public void Create(string id, Deal obj)
        {
            Items.Add(id, obj);
        }

        /// <inheritdoc />
        public Deal Get(string id)
        {
            Deal obj;
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
