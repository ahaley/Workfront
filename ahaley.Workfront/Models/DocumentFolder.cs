using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class DocumentFolder : WorkfrontResource
    {
        public override string ResourceToken { get { return "docfdr"; } }

        public static readonly string[] Fields = new string[] {
            "ID", "name", "objCode", "parentID", "taskID", "projectID", "issueID",
            "documents:downloadURL", "documents:docObjCode", "documents:description", "documents:issue:name",
            "project:name"
        };

        public string ParentID { get; set; }

        public string TaskID { get; set; }

        public string ProjectID { get; set; }

        public string IssueID { get; set; }

        public Document[] Documents { get; set; }

        public Project Project { get; set; }
    }
}
