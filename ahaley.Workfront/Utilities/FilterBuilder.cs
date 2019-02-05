using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ahaley.Workfront.Utilities
{
    public class FilterBuilder : IEquatable<FilterBuilder>
    {
        public FilterBuilder()
        {
            _filters = new List<string>();
            ContainsDateRange = false;
        }

        public List<string> Filter
        {
            get { return _filters; }
        }

        public string Uri
        {
            get {
                var sb = new StringBuilder();
                var parameters = new List<string>(Filter);
                if (Fields != null && Fields.Count() > 0) {
                    parameters.Add(string.Join("=", "fields", string.Join(",", Fields)));
                }
                if (!string.IsNullOrWhiteSpace(ApiKey)) {
                    parameters.Add(string.Join("=", "apiKey", ApiKey));
                }
                sb.Append(string.Join("&", parameters));
                return sb.ToString();
            }
        }

        private readonly List<string> _filters;

        public bool ContainsDateRange { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public string ApiKey { get; set; }

        public string[] Fields { get; set; }

        public bool Equals(FilterBuilder builder)
        {
            var left = _filters;
            var right = builder._filters;
            if (left.Count != right.Count)
                return false;
            return left.All(l => right.Any(r => l == r));
        }

        public FilterBuilder AddConstraint(string field, string value)
        {
            _filters.Add(string.Join("=", field, value));
            return this;
        }

        FilterBuilder AddModifier(string field, string opcode)
        {
            return AddConstraint(string.Join("_", field, "Mod"), opcode);
        }

        FilterBuilder AddRange(string field, DateTime date)
        {
            return AddConstraint(string.Join("_", field, "Range"), date.ToAtTaskDate());
        }

        public FilterBuilder ApplyOperation(string field, string value, string opcode)
        {
            AddConstraint(field, value);
            AddModifier(field, opcode);
            return this;
        }

        public FilterBuilder AddConstraint(string field, DateTime date)
        {
            if (ContainsDateRange) {
                if (date > EndDate)
                    EndDate = date;
                var weekPrior = date.AddDays(-7);
                if (weekPrior < StartDate)
                    StartDate = weekPrior;
            }
            else {
                EndDate = date;
                StartDate = date.AddDays(-7);
                ContainsDateRange = true;
            }
            return AddConstraint(field, date.ToAtTaskDate());
        }

        public FilterBuilder NotEquals(string field, string value)
        {
            return ApplyOperation(field, value, "ne");
        }

        public FilterBuilder DateRange(string field, DateTime startDate, DateTime endDate)
        {
            ContainsDateRange = true;
            StartDate = startDate;
            EndDate = endDate;
            AddConstraint(field, startDate.ToAtTaskDate());
            AddRange(field, endDate);
            return this;
        }

        public FilterBuilder ShortDateRange(string field, DateTime startDate, DateTime endDate)
        {
            ContainsDateRange = true;
            StartDate = startDate;
            EndDate = endDate;
            AddConstraint(field, startDate.ToShortDateString());
            AddConstraint(string.Join("_", field, "_Range"), endDate.ToShortDateString());
            return this;
        }

        public FilterBuilder GreaterThanOrEqual(string field, string value)
        {
            return ApplyOperation(field, value, "gte");
        }

        public FilterBuilder GreaterThanOrEqual(string field, DateTime date)
        {
            return GreaterThanOrEqual(field, date.ToAtTaskDate());
        }

        public FilterBuilder LessThanOrEqual(string field, string value)
        {
            return ApplyOperation(field, value, "lte");
        }

        public FilterBuilder LessThanOrEqual(string field, DateTime date)
        {
            return LessThanOrEqual(field, date.ToAtTaskDate());
        }

        public FilterBuilder NotNull(string field)
        {
            return AddConstraint(string.Join("_", field, "Mod"), "notnull");
        }

        public FilterBuilder IsNull(string field)
        {
            return AddConstraint(string.Join("_", field, "Mod"), "isnull");
        }
    }
}
