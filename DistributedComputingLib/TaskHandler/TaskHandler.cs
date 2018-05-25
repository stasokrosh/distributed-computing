using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace DistributedComputing
{
    public class TaskHandler<T,V,Divider,Compiler,Process> : IDisposable 
        where Divider : IObjectDivider<T>,new() 
        where Compiler : IResultCompiler<V>,new()
        where Process : IProcess<T,V>,new()
                                                            
    {
        private static ClientTask Task = null;
        private Uri LocalUri;
        private DividedResourceManager<T, Divider> DividedResourceManager;
        private bool Disposed = false;
        internal TaskHandler(TaskHandlerFactory factory)
        {
            if (Task == null)
            {
                Task = factory.ServerConnection.CreateTask(typeof(Process).Name, LibraryCreator.GetFiles(typeof(Process)));
            }
            LocalUri = factory.ServerConnection.LocalUri;
            DividedResourceManager = new DividedResourceManager<T, Divider>(LocalUri);
        }
        public V Execute(T data)
        {
            if (!Disposed)
            {
                DividedResource dividedResource = DividedResourceManager.CreateDividedResource(data);
                try
                {
                    List<DistributedComputingResult> distributedResults = null;
                    try
                    {
                        distributedResults = Task.ExecuteTask(dividedResource);
                    }
                    catch
                    {
                        throw new NoServerConnectionException();
                    }
                    if (distributedResults == null)
                    {
                        throw new TaskException();
                    }
                    List<V> results = new List<V>();
                    var serializer = new DataContractSerializer(typeof(V));
                    foreach (DistributedComputingResult distributedResult in distributedResults)
                    {
                        bool read = false;
                        DistributedResource resource = distributedResult.Result;
                        while (!read)
                        {
                            try
                            {
                                byte[] array = resource.Get();
                                results.Add((V)serializer.ReadObject(new MemoryStream(array)));
                                distributedResult.Result.Dispose();
                                read = true;
                            }
                            catch (Exception e)
                            {
                                resource = Task.Execute(distributedResult.Data);
                                if (resource == null)
                                {
                                    throw new NoServerConnectionException();
                                }
                            }
                        }
                    }
                    return new Compiler().CompileResult(results);
                }
                finally
                {
                    dividedResource.Dispose();
                }
            }
            else
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }
        public void Dispose()
        {
            DividedResourceManager.Dispose();
            Disposed = true;
        }
    }
}
