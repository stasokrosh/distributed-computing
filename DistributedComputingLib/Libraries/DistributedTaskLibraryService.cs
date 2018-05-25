using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DistributedComputing
{

    internal class DistributedTaskLibraryService : IDistributedTaskLibraryService
    {
        public byte[] Get(string filename)
        {
            return File.ReadAllBytes(filename);
        }
    }
}
