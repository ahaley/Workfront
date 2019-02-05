using System;

namespace ahaley.Workfront.Integration
{
    public class ConfigCreator
    {
        protected WorkfrontConfiguration CreateConfig()
        {
            return new WorkfrontConfiguration() {
                Username = "",
                Password = "",
                UrlPrefix = ""
            };
        }
    }
}
