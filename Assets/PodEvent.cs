using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [System.Serializable]
    public partial class PodEvent
    {
        public string type;
        public Object Object;
    }
    [System.Serializable]
    public partial class Object
    {
        public string kind;
        public string apiVersion;
        public Metadata metadata;
        public Spec spec;
    }

    [System.Serializable]
    public partial class Metadata
    {
        public string name;
        public string Namespace;
        public Guid uid;
        public DateTimeOffset creationTimestamp;
        public DateTimeOffset deletionTimestamp;
    }

    [System.Serializable]
    public class Limits
    {
        public string cpu;
        public string memory;
    }

    [System.Serializable]
    public class Requests
    {
        public string cpu;
        public string memory;
    }

    [System.Serializable]
    public class Resources
    {
        public Limits limits;
        public Requests requests;
    }

    [System.Serializable]
    public class Container
    {
        public string name;
        public string image;
        public Resources resources;
        public string terminationMessagePath;
        public string terminationMessagePolicy;
        public string imagePullPolicy;
    }

    [System.Serializable]
    public class Spec
    {
        public List<Container> containers;
        public string restartPolicy;
        public int terminationGracePeriodSeconds;
        public string dnsPolicy;
        public string serviceAccountName;
        public string serviceAccount;
        public string nodeName;
        public string schedulerName;
        public int priority;
        public bool enableServiceLinks;
    }
}