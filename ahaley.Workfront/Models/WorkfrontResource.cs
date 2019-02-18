using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public abstract class WorkfrontResource
    {
        public abstract string ResourceToken { get; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string ObjCode { get; set; }
    }
}
