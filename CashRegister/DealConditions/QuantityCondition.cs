using CashRegister.Models;

namespace CashRegister.DealConditions
{
    /// <summary>
    ///  Represents a condition in which the customer's
    ///  cart must certain a certain quantity of an item
    ///  for it to be considered satisfied.
    /// </summary>
    public class QuantityCondition : ICondition
    {
        /// <summary>
        ///  Product that must be present in cart.
        /// </summary>
        private Product _product;

        /// <summary>
        ///  Reference to cart object
        /// </summary>
        private Cart _cart;

        /// <summary>
        ///  Quantity required in order to be considered satisfied.
        /// </summary>
        private int _requiredQuantity;

        /// <summary>
        ///  Constructs this condition
        /// </summary>
        /// <param name="product">Product that must be present in cart.</param>
        /// <param name="cart">Reference to cart object</param>
        /// <param name="requiredQuantity">
        ///  Quantity required in order to be considered satisfied.
        /// </param>
        public QuantityCondition(Product product, Cart cart, int requiredQuantity)
        {
            _product = product;
            _cart = cart;
            _requiredQuantity = requiredQuantity;
        }

        /// <inheritdoc />
        public int TimesSatisfied()
        {
            int quantity = 0;
            if (_cart.Products.TryGetValue(_product.Sku, out quantity)) {
                if (quantity >= _requiredQuantity)
                {
                    return quantity / _requiredQuantity;
                }
            }
            return 0;
        }
    }
}
