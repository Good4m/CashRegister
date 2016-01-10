using CashRegister.DealConditions;
using System.Collections.Generic;

namespace CashRegister
{
    /// <summary>
    ///  - Represents a deal such as a discount or a coupon.
    ///  - Each deal has a number of conditions that need to be met
    ///    in order for it to be considered applyable.
    ///  - You can add a number of different conditions which makes
    ///    this class fairly flexible.
    ///  - You can chain add conditions:
    ///    eg. deal.AddCondition(condition).AddCondition(condition)
    /// </summary>
    public class Deal
    {
        /// <summary>
        ///  List of conditions that this deal must meet in order
        ///  to be applied to the customer's bill.
        /// </summary>
        private List<ICondition> _conditions = new List<ICondition>();

        /// <summary>
        /// Types of deals available.
        /// </summary>
        public enum DealType { Discount, Coupon };

        /// <summary>
        /// The type of this deal.
        /// </summary>
        public DealType Type { get; set; }

        /// <summary>
        ///  The monetary value that this deal is worth
        ///  if all conditions are satisfied.
        /// </summary>
        public decimal DealValue { get; set; }

        /// <summary>
        /// Constructs this deal.
        /// </summary>
        /// <param name="type">Type of this deal.</param>
        /// <param name="value">Monetary value of this deal.</param>
        public Deal(DealType type, decimal value = 0)
        {
            Type = type;
            DealValue = value;
        }

        /// <summary>
        /// Adds to the list of conditions that must be
        /// met in order for this deal to be applyable.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Deal AddCondition(ICondition condition)
        {
            if (condition != null)
                _conditions.Add(condition);
        
            return this;
        }

        /// <summary>
        /// Number of times this deal can be 
        /// applied to the customer's bill.
        /// </summary>
        /// <returns></returns>
        public int GetTimesApplyable()
        {
            if (_conditions.Count == 0)
                return 0;

            int times = 0;

            foreach (ICondition condition in _conditions)
            {
                times += condition.TimesSatisfied();
            }

            return times;
        }
    }
}
