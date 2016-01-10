using System;
using System.Collections.Generic;

namespace CashRegister.Models
{
    /// <summary>
    /// Represents a tangible product in the store.
    /// </summary>
    public class Product
    {
        /// <summary>
        ///  Unique product Sku.
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        ///  Name of product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Price when product is NOT on sale.
        /// </summary>
        public decimal RegPrice { get; set; }

        /// <summary>
        ///  Price when product is on sale.
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        ///  The actual price to charge the customer.
        ///  This will be either RegPrice or SalePrice depending
        ///  on if the product is on sale.
        /// </summary>
        public decimal ActualPrice
        {
            get
            {
                return IsOnSale ? SalePrice : RegPrice;
            }
        }

        /// <summary>
        ///  Weight of product.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        ///  Returns true if this product is on sale.
        /// </summary>
        public bool IsOnSale { get; set; }

        /// <summary>
        ///  Signifies how the register should calculate the price of a product.
        ///  - Unit is by quantity
        ///  - Weight is by lbs.
        /// </summary>
        public enum ChargeType { Unit, Weight };

        /// <summary>
        ///  How this product's price should be calculated.
        /// </summary>
        public ChargeType ChargeBy { get; set; }

        /// <summary>
        ///  Constructs this product.
        /// </summary>
        /// <param name="sku">Unique product Sku.</param>
        /// <param name="name">Name of product.</param>
        /// <param name="regPrice">Price if product is NOT on sale.</param>
        /// <param name="salePrice">Price when product is on sale.</param>
        /// <param name="chargeBy">How price should be calculated.</param>
        /// <param name="weight">Weight in lbs.</param>
        /// <param name="isOnSale">Is the product on sale?</param>
        public Product(string sku, string name, decimal regPrice, decimal salePrice,
            ChargeType chargeBy, decimal weight = 0, bool isOnSale = false)
        {
            Sku = sku;
            Name = name;
            RegPrice = regPrice;
            SalePrice = salePrice;
            ChargeBy = chargeBy;
            Weight = weight;
            IsOnSale = isOnSale;
        }
    }
}
