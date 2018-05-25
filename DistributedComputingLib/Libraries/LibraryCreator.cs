using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace DistributedComputing
{
    internal static class LibraryCreator
    {
        public static List<string> GetFiles(Type type)
        {
            Assembly assembly = type.Assembly;
            var result = new List<string>();
            GetAssemblyFiles(assembly, result);
            return result;
        }
        private static void GetAssemblyFiles(Assembly assembly,List<string> files)
        {
            files.Add(assembly.Location);
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach (AssemblyName reference in referencedAssemblies)
            {
                if (reference.CodeBase != null && reference != Assembly.GetExecutingAssembly().GetName())
                {
                    var referencedAssembly = Assembly.LoadFrom(reference.CodeBase);
                    if (!files.Contains(referencedAssembly.Location))
                    {
                        GetAssemblyFiles(referencedAssembly, files);
                    }
                }
            }
        } 
    }
}
