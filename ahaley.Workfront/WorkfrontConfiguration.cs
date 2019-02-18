using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ahaley.Workfront
{
    public class WorkfrontConfiguration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string UrlPrefix { get; set; }

        public string SessionID { get; set; }

        public string VersionedUrl
        {
            get {
                return string.Join("/", UrlPrefix, "attask/api/v10.0/");
            }
        }

        public static WorkfrontConfiguration CreateConfiguration(NameValueCollection appSettings = null)
        {
            if (appSettings == null) {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                appSettings = ConfigurationManager.AppSettings;
            }

            return new WorkfrontConfiguration() {
                Username = appSettings.Get("Username"),
                Password = appSettings.Get("Password"),
                UrlPrefix = appSettings.Get("AtTaskUrl")
            };
        }
    }
}
