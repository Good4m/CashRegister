using CashRegister.Models;
using System.Collections.Generic;

namespace CashRegister
{
    /// <summary>
    ///  This class is in charge of calculating a customer's bill.
    /// </summary>
    public class Register
    {
        /// <summary>
        ///  Total before deductions.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        ///  Total taxes to be applied.
        /// </summary>
        public decimal TaxesTotal { get; set; }

        /// <summary>
        ///  Total discounts to be applied.
        /// </summary>
        public decimal DiscountsTotal { get; set; }

        /// <summary>
        ///  Total coupons to be applied.
        /// </summary>
        public decimal CouponsTotal { get; set; }

        /// <summary>
        ///  Total amount to be paid by the customer.
        /// </summary>
        public decimal TotalDue { get; set; }

        /// <summary>
        ///  Reference to the shopping cart.
        /// </summary>
        private Cart _cart;

        /// <summary>
        ///  Reference to the product object.
        /// </summary>
        private Products _products;

        /// <summary>
        ///  Reference to the deals object.
        /// </summary>
        private Deals _deals;

        /// <summary>
        ///  Initializes the cash register
        /// </summary>
        /// <param name="cart">Reference to the cart object.</param>
        /// <param name="products">Reference to the products object.</param>
        /// <param name="deals">Reference to the deals object.</param>
        public Register(Cart cart, Products products, Deals deals)
        {
            _cart = cart;
            _products = products;
            _deals = deals;
        }

        /// <summary>
        ///  Calculates every total at once.
        /// </summary>
        public void CalculateAllTotals()
        {
            // Reset all totals
            ResetRegister();

            // Calculate subtotal
            SubTotal = CalculateSubtotal();

            // Calculate discounts total
            DiscountsTotal = CalculateDiscountsTotal();

            // Calculate coupons total
            CouponsTotal = CalculateCouponsTotal();

            // Calculate taxes
            TaxesTotal = CalculateTaxes();

            // Calculate total amount due
            TotalDue = CalculateTotalDue();
        }

        /// <summary>
        ///  Calculates the subtotal.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateSubtotal()
        {
            decimal subtotal = 0;
            foreach (KeyValuePair<string, int> entry in _cart.Products)
            {
                Product product = _products.Get(entry.Key);
                int quantity = entry.Value;
                if (product.ChargeBy == Product.ChargeType.Unit)
                {
                    subtotal += (product.ActualPrice * quantity);
                }
                else if (product.ChargeBy == Product.ChargeType.Weight) {
                    //subtotal += (product.ActualPrice * quantity);
                    subtotal += (product.Weight * product.ActualPrice * quantity);
                }
            }
            return subtotal;
        }

        /// <summary>
        ///  Calculates the discounts total.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateDiscountsTotal()
        {
            // Calculate discounts total
            decimal discountsTotal = 0;
            foreach (KeyValuePair<string, Deal> entry in _deals.All())
            {
                int timesSatisfied = entry.Value.GetTimesApplyable();

                if (timesSatisfied == 0)
                    continue;

                if (entry.Value.Type == Deal.DealType.Discount)
                    discountsTotal += (entry.Value.DealValue * timesSatisfied);
            }
            return discountsTotal;
        }

        /// <summary>
        ///  Calculates the coupons total.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateCouponsTotal()
        {
            decimal couponsTotal = 0;
            // Calculate coupons total
            foreach (KeyValuePair<string, Deal> entry in _deals.All())
            {
                int timesSatisfied = entry.Value.GetTimesApplyable();

                if (timesSatisfied == 0)
                    continue;

                if (entry.Value.Type == Deal.DealType.Coupon)
                    couponsTotal += (entry.Value.DealValue * timesSatisfied);
            }
            return couponsTotal;
        }

        /// <summary>
        ///  Calculates the total amount to be paid by the customer.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateTotalDue()
        {
            return SubTotal - DiscountsTotal - CouponsTotal + TaxesTotal;
        }

        /// <summary>
        ///  Calculates the taxes total.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateTaxes()
        {
            return (SubTotal - DiscountsTotal - CouponsTotal) * (Tax.GST + Tax.PST);
        }

        /// <summary>
        /// Resets all totals to zero.
        /// </summary>
        private void ResetRegister()
        {
            SubTotal = 0;
            TaxesTotal    = 0;
            DiscountsTotal = 0;
            CouponsTotal  = 0;
            TotalDue    = 0;
        }
    }
}
