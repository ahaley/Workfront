using System;

namespace ahaley.Workfront
{
    class ResultResponse
    {
        public string Result { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(Result); }
        }
    }
}
