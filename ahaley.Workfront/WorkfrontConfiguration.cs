﻿using System;

namespace ahaley.Workfront
{
    public class WorkfrontConfiguration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string UrlPrefix { get; set; }

        public string VersionedUrl
        {
            get {
                return UrlPrefix + "v9.0/";
            }
        }
    }
}
