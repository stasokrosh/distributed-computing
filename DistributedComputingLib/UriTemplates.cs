using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DistributedComputing
{
    internal static class UriTemplates
    { 
        public static Uri CreateClientRegistrationUri(Uri baseUri)
        {
            return new Uri(baseUri + "registration/client");
        }
        public static Uri CreateWorkerRegistrationUri(Uri baseUri)
        {
            return new Uri(baseUri + "registration/worker");
        }
        public static Uri CreateCheckConnectionUri(Uri baseUri)
        {
            return new Uri(baseUri + "checkconnection/");
        }
        public static Uri CreateTasksUri(Uri baseUri)
        {
            return new Uri(baseUri + "tasks/");
        }
        public static Uri CreateLibrariesUri(Uri baseUri)
        {
            return new Uri(baseUri + "libraries/");
        }
        public static Uri CreateResourcesUri(Uri baseUri)
        {
            return new Uri(baseUri + "resources/");
        }
        public static Uri CreateDividedResourcesUri(Uri baseUri)
        {
            return new Uri(baseUri + "dividedresources/");
        }
        public static Uri CreatePerformanceUri(Uri baseUri)
        {
            return new Uri(baseUri + "performance/");
        }
    }
}
