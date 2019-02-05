using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ahaley.Workfront.Utilities
{
    class WorkfrontDateConverter : IsoDateTimeConverter
    {
        public WorkfrontDateConverter()
        {
            base.DateTimeFormat = WorkfrontExtentionMethods.AtTaskDateTimeFormat;
        }
    }
}
