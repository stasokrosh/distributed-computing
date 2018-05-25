using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;

namespace DistributedComputing
{
    internal class TaskLibraryManager
    {
        internal List<TaskLibrary> Tasks; 
        private string Filename = "Tasks.xml";
        private string Folder = "Tasks";
        private object Locker = new object();
        public void Load()
        {
            if (File.Exists(Filename))
            {
                var serializer = new DataContractSerializer(typeof(List<TaskLibrary>));
                using (FileStream stream = new FileStream(Filename, FileMode.Open))
                {
                    Tasks = (List<TaskLibrary>)serializer.ReadObject(stream);
                }
                var removedLibs = new List<TaskLibrary>();
                foreach (TaskLibrary library in Tasks)
                {
                    library.Load();
                    foreach (string filename in library.Files)
                    {
                        if (!File.Exists(filename))
                        {
                            removedLibs.Add(library);
                            break;
                        }
                    }
                }
                foreach (TaskLibrary library in removedLibs)
                {
                    Tasks.Remove(library);
                }
            }
            else
            {
                Tasks = new List<TaskLibrary>();
            }
        }
        public int CreateTask(DistributedTaskLibrary distributedLibrary)
        {
            int taskId = -1;
            var Service = RemoteConnection.CreateProxy<IDistributedTaskLibraryService>(UriTemplates.CreateLibrariesUri(distributedLibrary.Holder));
            lock (Locker)
            {
                File.WriteAllBytes("temp",Service.Get(distributedLibrary.Files[0]));
                var assembly = Assembly.LoadFrom("temp");
                Type processType = assembly.GetTypes().Where(type => type.Name == distributedLibrary.ProcessTypeName).First();
                for(int i=0;i<Tasks.Count;i++)
                {
                    if (Tasks[i].Process == processType)
                    {
                        taskId = i;
                        break;
                    }
                }
            }
            if (taskId == -1)
            {
                int i = 1;
                string folder;
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                while (Directory.Exists(folder = (Folder+"/"+"task" + i)))
                {
                    i++;
                }
                Directory.CreateDirectory(folder);
                List<string> files = new List<string>();
                foreach (string filename in distributedLibrary.Files)
                {
                    string name = folder + "/" + new DirectoryInfo(filename).Name;
                    File.WriteAllBytes(name, Service.Get(filename));
                    files.Add(name);
                }
                Tasks.Add(new TaskLibrary(files, distributedLibrary.ProcessTypeName));
                taskId = Tasks.Count - 1;
                Save();
            }
            return taskId;
        }
        public void Save()
        {
            var serializer = new DataContractSerializer(typeof(List<TaskLibrary>));
            using (FileStream stream = new FileStream(Filename, FileMode.Create))
            {
                serializer.WriteObject(stream,Tasks);
            }
        }
    }
}
