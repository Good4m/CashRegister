
namespace CashRegister.DealConditions
{
    /// <summary>
    ///  Specifies the one method that each condition
    ///  must implement in order to allow for polymorphism
    ///  when calculating deals.
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// The number of times this condition is satisfiable.
        /// </summary>
        /// <returns></returns>
        int TimesSatisfied();
    }
}
