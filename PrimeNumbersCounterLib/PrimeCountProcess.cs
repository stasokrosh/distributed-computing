using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedComputing;

namespace PrimeNumbersCounterLib
{
    public class PrimeCountProcess : IProcess<PrimeNumbersProgression,List<int>>
    {
        public List<int> Execute(PrimeNumbersProgression progression)
        {
            var result = new List<int>();
            if (progression.BaseNum == 7)
            {
                foreach (int num in PrimeNumbersBase.BasePrimes)
                {
                    if (num <= progression.Max)
                    {
                        result.Add(num);
                    }
                }
            }
            int currentNum = progression.BaseNum;
            while (currentNum <= progression.Max)
            {
                int squareRoot = (int)Math.Sqrt(currentNum);
                bool found = false;
                int i = 0;
                while (i < PrimeNumbersBase.BasePrimes.Count && !found)
                {
                    if (currentNum % PrimeNumbersBase.BasePrimes[i] == 0)
                    {
                        found = true;
                    }
                    i++;
                }
                List<int> progressions = new List<int>();
                foreach (int num in PrimeNumbersBase.BaseProgressionPrimes)
                {
                    {
                        progressions.Add(num);
                    }
                }
                while (progressions[0] <= squareRoot && !found)
                {
                    i = 0;
                    while (i<progressions.Count && progressions[i]<= squareRoot && !found)
                    {
                        if (currentNum % progressions[i] == 0)
                        {
                            found = true;
                        }
                        progressions[i++] += PrimeNumbersBase.ProgressionStep;
                    }
                }
                if (!found)
                {
                    result.Add(currentNum);
                }
                currentNum += PrimeNumbersBase.ProgressionStep;
            }
            return result;
        }
    }
}
