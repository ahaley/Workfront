using ahaley.Workfront.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class AccessRule
    {
        public string AccessorID { get; set; }

        public string AccessorObjCode { get; set; }
    }

    public class Document : WorkfrontResource
    {
        public override string ResourceToken { get { return "document"; } }

        public static readonly string[] Fields = new string[] {
            "ID", "name", "docObjCode", "description", "lastUpdateDate", "hasNotes", "ownerID"
        };

        public static readonly string[] RequiredFieldsForArchiving = new string[] {
            "ID", "name", "docObjCode", "description", "lastUpdateDate", "hasNotes", "ownerID", "downloadURL",
            "isPrivate", "isPublic",
            "projectID", "opTaskID", "userID",
            "project:ID", "project:name", "project:objCode",
            "opTask:ID", "opTask:name", "opTask:objCode",
            "folders:ID", "folders:objCode", "folders:name", "folders:parentID", "folders:taskID", "folders:projectID",
            "user:ID", "user:name", "user:firstName", "user:lastName",
            "accessRules:accessorID", "accessRules:accessorObjCode"
        };

        public static readonly string[] AllFields = new string[] {
            "*", "folders:*", "user:*", "opTask:*", "task:*", "groups:*"
        };

        public static readonly string[] SubscriberOnly = new string[] { "groups:*", "subscribers:*" };

        public string DocObjCode { get; set; }

        public string ProjectId { get; set; }

        [JsonProperty("opTaskID")]
        public string IssueId { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime LastUpdateDate { get; set; }

        public bool HasNotes { get; set; }

        public string UserID { get; set; }

        public string OwnerID { get; set; }

        public bool IsDir { get; set; }

        public string DownloadURL { get; set; }

        public bool IsPublic { get; set; }
        
        public bool IsPrivate { get; set; }

        public IEnumerable<DocumentFolder> Folders { get; set; }

        public Project Project { get; set; }

        [JsonProperty("opTask")]
        public Issue Issue { get; set; }

        public WorkfrontTask Task { get; set; }

        public User User { get; set; }

        public Group[] Groups { get; set; }

        public User[] Subscribers { get; set; }

        public AccessRule[] AccessRules { get; set; }
    }
}
