using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    public class Utilities
    {
        public static string FormatAsMoney(decimal dec)
        {
            return string.Format("{0:C}", dec);
        }
    }
}
