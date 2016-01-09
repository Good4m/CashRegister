using CashRegister.Coupons;
using CashRegister.Coupons.Rules;
using CashRegister.Models;
using System.Collections.Generic;

namespace CashRegister
{
    class Register
    {
        public decimal SubTotal { get; set; }
        public decimal Taxes    { get; set; }
        public decimal Discount { get; set; }
        public decimal Total    { get; set; }
        
        public void Checkout(Cart cart)
        {
            ResetRegister();

            foreach (KeyValuePair<string, int> entry in cart.Products)
            {
                Product product = Product.Get(entry.Key);

                SubTotal += product.RegPrice;

                if (product.IsOnSale)
                    Discount += (product.RegPrice - product.SalePrice);
            }

            Taxes = SubTotal * (Tax.GST + Tax.PST);
            Total = SubTotal + Taxes - Discount;

            Coupon coupon = new Coupon();
            coupon.AddCondition(new QuantityCondition())
                  .AddCondition(new ThreshholdCondition());

            if (coupon.IsUsable())
                
        }

        private void ResetRegister()
        {
            SubTotal = 0;
            Taxes    = 0;
            Discount = 0;
            Total    = 0;
        }
    }
}
