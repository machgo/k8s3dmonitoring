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
}
