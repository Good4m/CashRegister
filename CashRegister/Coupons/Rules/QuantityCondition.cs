using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Coupons.Rules
{
    class QuantityCondition : Condition
    {
        public decimal Param1 { get; set; }
        public decimal Param2 { get; set; }

        public bool IsSatisfied()
        {
            throw new NotImplementedException();
        }
    }
}
