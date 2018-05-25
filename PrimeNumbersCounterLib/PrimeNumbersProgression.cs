using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PrimeNumbersCounterLib
{
    [DataContract]
    public class PrimeNumbersProgression
    {
        [DataMember]
        public int BaseNum { get; private set; }
        [DataMember]
        public int Max { get; private set; }
        public PrimeNumbersProgression(int max, int baseNum = 0)
        {
            BaseNum = baseNum;
            Max = max;
        }
    }
}
