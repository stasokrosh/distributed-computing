using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedComputing;

namespace PrimeNumbersCounterLib
{
    public class PrimeNumbersCompiler : IResultCompiler<List<int>>
    {
        public List<int> CompileResult(List<List<int>> results)
        {
            var compiledResult = new List<int>();
            foreach (List<int> result in results)
            {
                compiledResult.AddRange(result);
            }
            return compiledResult;  
        }
    }
}
