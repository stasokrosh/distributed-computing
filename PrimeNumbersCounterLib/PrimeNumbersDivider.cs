using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedComputing;

namespace PrimeNumbersCounterLib
{
    public class PrimeNumbersDivider : IObjectDivider<PrimeNumbersProgression>
    {
        public List<PrimeNumbersProgression> Divide(PrimeNumbersProgression progression)
        {
            var result = new List<PrimeNumbersProgression>();
            foreach (int num in PrimeNumbersBase.BaseProgressionPrimes)
            {
                result.Add(new PrimeNumbersProgression(progression.Max, num));
            }
            return result;
        }
    }
}
