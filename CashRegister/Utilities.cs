using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    /// <summary>
    /// Holds useful helper methods.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        ///  Converts a decimal 1.1111 to 1.11.
        ///  Also automatically inserts a $ sign if printing to console.
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string FormatAsMoney(decimal dec)
        {
            return string.Format("{0:C}", dec);
        }
    }
}
