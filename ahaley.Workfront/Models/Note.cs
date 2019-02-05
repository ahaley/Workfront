using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ahaley.Workfront.Utilities;

namespace ahaley.Workfront.Models
{
    public class Note
    {
        public string ID { get; set; }

        public Note[] Replies { get; set; }

        public string ObjCode { get; set; }

        public string AttachObjCode { get; set; }

        public string AttachObjID { get; set; }

        public string AuditText { get; set; }

        public string AuditType { get; set; }

        public string CustomerID { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? EntryDate { get; set; }

        public bool HasReplies { get; set; }

        public int Indent { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsMessage { get; set; }
        
        public bool IsPrivate { get; set; }

        public string NoteObjCode { get; set; }

        public string NoteText { get; set; }

        public int NumReplies { get; set; }

        public string OwnerID { get; set; }

        public string ParentJournalEntryID { get; set; }

        public string ParentNoteID { get; set; }

        public string Subject { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ThreadDate { get; set; }
        
        public string ThreadID { get; set; }

        public bool IsArchived { get; set; }

        public bool IsTopArchived { get; set; }
    }
}
