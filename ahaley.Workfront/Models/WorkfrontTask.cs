using ahaley.Workfront.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ahaley.Workfront.Models
{
    public class WorkfrontTask : WorkfrontResource
    {
        public override string ResourceToken { get { return "task"; } }

        public static readonly string[] Fields = new string[] {
            "*", "group:*", "documents:*"
        };

        public int TaskNumber { get; set; }

        public string ParentID { get; set; }

        public double PercentComplete { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ActualCompletionDate { get; set; }

        public decimal ActualCost { get; set; }

        public decimal ActualDuration { get; set; }

        public int ActualDurationMinutes { get; set; }

        public decimal ActualExpenseCost { get; set; }

        public decimal ActualLaborCost { get; set; }

        public decimal ActualRevenue { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ActualStartDate { get; set; }

        public double ActualWork { get; set; }

        public int ActualWorkRequired { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ApprovalEstStartDate { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ApprovalPlannedStartDate { get; set; }

        public string ApprovalProcessID { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? ApprovalProjectedStartDate { get; set; }

        public string AssignedToID { get; set; }

        public decimal BillingAmount { get; set; }

        public string BillingRecordID { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? CompletionPendingDate { get; set; }

        public decimal CostAmount { get; set; }

        public string CostType { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? EstCompletionDate { get; set; }

        [JsonConverter(typeof(WorkfrontDateConverter))]
        public DateTime? EstStartDate { get; set; }

        public double Estimate { get; set; }

        public double Work { get; set; }

        public int WorkRequired { get; set; }

        public Group Group { get; set; }

        public Document[] Documents { get; set; }
        
    }
}
