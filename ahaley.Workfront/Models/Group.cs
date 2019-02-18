using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class Group : WorkfrontResource
    {
        public override string ResourceToken { get { return "group"; } }

        public string Description { get; set; }
    }
}
