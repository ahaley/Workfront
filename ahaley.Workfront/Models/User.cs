using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class User : WorkfrontResource
    {
        public override string ResourceToken { get { return "user"; } }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddr { get; set; }
    }
}
