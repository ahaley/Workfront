using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class Issue : WorkfrontResource
    {
        public override string ResourceToken { get { return "opTask"; } }
    }
}
