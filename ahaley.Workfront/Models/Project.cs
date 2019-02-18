using ahaley.Workfront.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ahaley.Workfront.Models
{
    public class Project : WorkfrontResource
    {
        public override string ResourceToken { get { return "project"; } }

        public static readonly string[] SalesOrderOnly = new string[] {
            "name"
        };

        public static readonly string[] Fields = new string[] {
            "ID", "name", "actualStartDate", "percentComplete", "plannedCompletionDate", "plannedStartDate",
            "priority", "projectedCompletionDate", "tasks:*"
        };

        public static readonly string[] DocumentFields = new string[] {
            "ID", "name", "documents:*", "documents:folders:*"
        };

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ActualStartDate { get; set; }

        public decimal PercentComplete { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime PlannedCompletionDate { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime PlannedStartDate { get; set; }

        public int Priority { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime ProjectedCompletionDate { get; set; }

        public string Status { get; set; }

        public IEnumerable<WorkfrontTask> Tasks { get; set; }

        public IEnumerable<Document> Documents { get; set; }
    }
}
