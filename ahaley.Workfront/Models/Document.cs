using ahaley.Workfront.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class WorkfrontResource
    {
        public string ID { get; set; }
        public string ObjCode { get; set; }
        public string Name { get; set; }

    }

    public class DocumentFolder : WorkfrontResource
    {
        public static readonly string[] Fields = new string[] {
            "ID", "name", "objCode", "parentID", "taskID", "projectID", "issueID", "documents:downloadURL,docObjCode"
        };

        public string ParentID { get; set; }

        public string TaskID { get; set; }

        public string ProjectID { get; set; }

        public string IssueID { get; set; }

        public Document[] Documents { get; set; }

    }

    public class Document : WorkfrontResource
    {
        public static readonly string[] Fields = new string[] {
            "ID", "name", "objCode", "description", "lastUpdateDate", "hasNotes", "ownerID"
        };

        public string Description { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime LastUpdateDate { get; set; }

        public bool HasNotes { get; set; }

        public string OwnerID { get; set; }

        public bool IsDir { get; set; }

    }
}
