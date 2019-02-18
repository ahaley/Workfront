using System;

namespace ahaley.Workfront
{
    class LoginResponse
    {
        public string Version { get; set; }

        public string Release { get; set; }

        public string CurrentAPI { get; set; }

        public string SessionID { get; set; }
    }

    class ResultResponse
    {
        public string Result { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(Result); }
        }
    }
}
