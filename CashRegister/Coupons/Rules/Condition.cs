using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Coupons
{
    abstract class Condition
    {
        decimal Param1 { get; set; }
        decimal Param2 { get; set; }

        public abstract bool IsSatisfied();
    }
}
