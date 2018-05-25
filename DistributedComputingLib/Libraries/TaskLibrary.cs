using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;

namespace DistributedComputing
{
    [DataContract]
    internal class TaskLibrary
    {
        [DataMember]
        public String ProcessTypeName { get; private set; }
        [DataMember]
        public List<string> Files { get; private set; }
        public Type Process { get; private set; }
        public Type InputData { get; private set;}
        public void Load()
        {
            var assembly = Assembly.LoadFrom(Files[0]);
            Process = assembly.GetTypes().Where(type => type.Name == ProcessTypeName).First();
            InputData = Process.GetInterfaces().Where(type => type.Name.Contains("IProcess")).First().GetGenericArguments()[0];
        }
        public TaskLibrary(List<string> files, string name)
        {
            ProcessTypeName = name; 
            Files = files;
            Load();
        }
        public DistributedTaskLibrary CreateDistributedTaskLibrary(Uri holder)
        {
            return new DistributedTaskLibrary(holder, Files, ProcessTypeName);
        }
    }
}
