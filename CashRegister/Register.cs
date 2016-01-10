using CashRegister.Models;
using System.Collections.Generic;

namespace CashRegister
{
    public class Register
    {
        public decimal SubTotal      { get; set; }
        public decimal TaxesTotal    { get; set; }
        public decimal DiscountsTotal { get; set; }
        public decimal CouponsTotal  { get; set; }
        public decimal TotalDue         { get; set; }

        private Cart _cart;
        private Products _products;
        private Deals _deals;

        public Register(Cart cart, Products products, Deals deals)
        {
            _cart = cart;
            _products = products;
            _deals = deals;
        }

        public decimal CalculateAllTotals()
        {
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

            return TotalDue;
        }

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

        public decimal CalculateTotalDue()
        {
            return SubTotal - DiscountsTotal - CouponsTotal + TaxesTotal;
        }

        public decimal CalculateTaxes()
        {
            return (SubTotal - DiscountsTotal - CouponsTotal) * (Tax.GST + Tax.PST);
        }

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
