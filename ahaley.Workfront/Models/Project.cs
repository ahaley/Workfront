using ahaley.Workfront.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ahaley.Workfront.Models
{
    public class Project
    {
        public static readonly string[] Fields = new string[] {
            "ID", "name", "objCode", "actualStartDate", "percentComplete", "plannedCompletionDate", "plannedStartDate",
            "priority", "projectedCompletionDate", "tasks:*"
        };

        public string ID { get; set; }

        public string Name { get; set; }

        public string ObjCode { get; set; }

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

        public IEnumerable<Task> Tasks { get; set; }
    }
}
