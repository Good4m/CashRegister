
namespace CashRegister.DealConditions
{
    /// <summary>
    ///  Represents a condition in which the customer's
    ///  subtotal must reach a certain threshhold in order
    ///  for it to be considered satisfied.
    /// </summary>
    public class CartTotalCondition : ICondition
    {
        /// <summary>
        ///  Reference to the register object
        /// </summary>
        private Register _register;

        /// <summary>
        ///  Threshhold the customer's subtotal must cross for this
        ///  condition to be satisfied.
        /// </summary>
        private decimal _amountRequired;
        
        /// <summary>
        ///  Constructs this condition
        /// </summary>
        /// <param name="register">Reference to register object</param>
        /// <param name="amountRequired">Threshhold to be considered satisfied</param>
        public CartTotalCondition(Register register, decimal amountRequired)
        {
            _register = register;
            _amountRequired = amountRequired;
        }

        /// <inheritdoc />
        public int TimesSatisfied()
        {
            decimal subtotal = _register.CalculateSubtotal();
            return (int)(subtotal / _amountRequired);
        }
    }
}
