using System;
using System.Collections.Generic;

namespace ahaley.Workfront.Utilities
{
    class WorkfrontUriBuilder
    {
        readonly WorkfrontConfiguration config;

        public WorkfrontUriBuilder(WorkfrontConfiguration config)
        {
            this.config = config;
        }

        public string CreateResourceUri(string resource, string id, FilterBuilder filterBuilder)
        {
            filterBuilder.ApiKey = config.Token;
            string uri = string.Join("?", string.Join("/", resource, id), filterBuilder.Uri);
            return uri;
        }

        public string CreateSearchUri(string resource, FilterBuilder filterBuilder)
        {
            filterBuilder.ApiKey = config.Token;
            string uri = string.Join("?", string.Join("/", resource, "search"), filterBuilder.Uri);
            return uri;
        }
    }
}
