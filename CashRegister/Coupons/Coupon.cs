using CashRegister.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    class Coupon
    {
        private List<Condition> _conditions = new List<Condition>();

        public Coupon AddCondition(Condition condition)
        {

            if (condition != null)
                _conditions.Add(condition);
        
            return this;
        }

        public bool IsUsable()
        {
            foreach (Condition condition in _conditions)
            {
                if (!condition.IsSatisfied())
                    return false;
            }
            return true;
        }
    }
}
