using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbersCounterLib
{
    public static class PrimeNumbersBase
    {
        public static readonly List<int> BasePrimes = new List<int>() { 3, 5 };
        public static readonly List<int> BaseProgressionPrimes = new List<int>() { 7, 11, 13, 17, 19, 23, 29, 31 }; 
        public static readonly int ProgressionStep = 30;
    }
}
