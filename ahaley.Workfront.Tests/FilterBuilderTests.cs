using ahaley.Workfront.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ahaley.Workfront.Tests
{
    [TestFixture]
    public class FilterBuilderTests
    {
        [Test]
        public void Builders_Should_Be_Equal_If_Filters_Match()
        {
            var builder1 = new FilterBuilder();
            var builder2 = new FilterBuilder();
            builder1.DateRange("startDate", new DateTime(2011, 1, 1), new DateTime(2011, 1, 12));
            builder2.DateRange("startDate", new DateTime(2011, 1, 1), new DateTime(2011, 1, 12));

            Assert.IsTrue(builder1.Equals(builder2));
            Assert.IsTrue(builder2.Equals(builder1));
            Assert.IsTrue(builder1.Equals(builder1));
        }

        [Test]
        public void Builders_Should_Not_Be_Equal_If_Filters_Do_Not_Match()
        {
            var builder1 = new FilterBuilder();
            var builder2 = new FilterBuilder();
            builder1.DateRange("startDate", new DateTime(2011, 1, 1), new DateTime(2011, 1, 12));
            builder2.DateRange("startDate", new DateTime(2011, 1, 1), new DateTime(2011, 1, 13));

            Assert.IsFalse(builder1.Equals(builder2));
            Assert.IsFalse(builder2.Equals(builder1));
        }

        [Test]
        public void Single_Field_Equals_Contructs_Correct_Filter()
        {
            var builder = new FilterBuilder();

            builder.AddConstraint("name1", "value1");

            List<string> filter = builder.Filter;
            Assert.AreEqual(1, filter.Count);
            Assert.IsTrue(filter.Contains("name1=value1"));
            Assert.AreEqual("name1=value1", builder.Uri);
        }

        [Test]
        public void FieldEquals_With_DateTime_Produces_AtTask_Date_String()
        {
            var builder = new FilterBuilder();
            var date = new DateTime(2011, 1, 1);

            builder.AddConstraint("test1", date);

            Assert.IsTrue(builder.Filter.Contains("test1=" + date.ToAtTaskDate()));
            Assert.AreEqual("test1=" + date.ToAtTaskDate(), builder.Uri);
        }

        [Test]
        public void Builder_Handles_Filter_Composition_Correctly()
        {
            var builder = new FilterBuilder();
            builder.AddConstraint("name1", "value1");
            builder.AddConstraint("name2", "value2");

            Assert.AreEqual("name1=value1&name2=value2", builder.Uri);
        }

        [Test]
        public void Builder_Constructs_Proper_Date_Range_Operation()
        {
            var builder = new FilterBuilder();
            var startDate = new DateTime(2010, 12, 1);
            var endDate = new DateTime(2010, 12, 31);

            builder.DateRange("startDate", startDate, endDate);

            Assert.AreEqual("startDate=2010-12-01T13:27:29:999&startDate_Range=2010-12-31T13:27:29:999",
                builder.Uri);

            List<string> filter = builder.Filter;
            Assert.AreEqual(2, filter.Count);
            Assert.IsTrue(filter.Contains("startDate=2010-12-01T13:27:29:999"));
            Assert.IsTrue(filter.Contains("startDate_Range=2010-12-31T13:27:29:999"));
        }

        [Test]
        public void Builder_Constructs_Proper_Date_Greater_Than_Operation()
        {
            var builder = new FilterBuilder();

            ExpectCorrectDateRangeFilter("startDate", "gte", (field, date) => {
                builder.GreaterThanOrEqual(field, date);
                return builder.Filter;
            });
        }

        [Test]
        public void Builder_Constructs_Proper_Date_Less_Than_Operation()
        {
            var builder = new FilterBuilder();

            ExpectCorrectDateRangeFilter("startDate", "lte", (field, date) => {
                builder.LessThanOrEqual(field, date);
                return builder.Filter;
            });
        }

        [Test]
        public void ContainsDateRange_Will_Return_True_If_Date_Range_Exists()
        {
            var builder = new FilterBuilder();
            var start = new DateTime(2010, 1, 1);
            var end = new DateTime(2010, 1, 15);
            builder.DateRange("endDate", start, end);

            Assert.IsTrue(builder.ContainsDateRange);
            Assert.AreEqual(start, builder.StartDate);
            Assert.AreEqual(end, builder.EndDate);
        }

        [Test]
        public void ContainsDateRange_Will_Return_False_If_Date_Range_Doesnt_Exist()
        {
            var builder = new FilterBuilder();

            Assert.IsFalse(builder.ContainsDateRange);
        }

        [Test]
        public void Given_Date_Equality_Date_Range_Will_Start_Week_Prior()
        {
            var builder = new FilterBuilder();
            var endDate = new DateTime(2011, 1, 16);
            
            builder.AddConstraint("endDate", endDate);

            Assert.IsTrue(builder.ContainsDateRange);
            Assert.AreEqual(endDate, builder.EndDate);
            Assert.AreEqual(endDate.AddDays(-7), builder.StartDate);
        }

        [Test]
        public void Given_Two_Date_Equalities_Start_And_End_Date_Are_Extents()
        {
            var builder = new FilterBuilder();
            var endDate1 = new DateTime(2011, 1, 16);
            var endDate2 = new DateTime(2011, 1, 23);
            builder.AddConstraint("endDate", endDate1);
            builder.AddConstraint("OR:a:endDate", endDate2);

            Assert.IsTrue(builder.ContainsDateRange);
            Assert.AreEqual(endDate2, builder.EndDate);
            Assert.AreEqual(endDate1.AddDays(-7), builder.StartDate);
        }

        [Test]
        public void Given_Only_EndDate_StartDate_Extent_Is_Week_Prior()
        {
            var builder = new FilterBuilder();
            var endDate = new DateTime(2011, 3, 27);
            builder.AddConstraint("endDate", endDate);

            Assert.IsTrue(builder.ContainsDateRange);
            Assert.AreEqual(endDate, builder.EndDate);
            Assert.AreEqual(endDate.AddDays(-7), builder.StartDate);
        }

        [Test]
        public void ApiKey_Appends_Key_To_End()
        {
            var builder = new FilterBuilder();

            builder.ApiKey = "12345";
            builder.AddConstraint("name1", "value1");

            Assert.AreEqual("name1=value1&apiKey=12345", builder.Uri);
            Assert.AreEqual("name1=value1&apiKey=12345", builder.Uri);
        }

        [Test]
        public void Fields_Appended_After_Filters_Before_ApiKey()
        {
            var builder = new FilterBuilder();
            builder.Fields = new string[] { "field1", "field2", "field3" };
            builder.ApiKey = "12345";
            builder.AddConstraint("name1", "value1");

            Assert.AreEqual("name1=value1&fields=field1,field2,field3&apiKey=12345",
                builder.Uri);

        }

        [Test]
        public void NotNull_Adds_Constraint()
        {
            var builder = new FilterBuilder();
            builder.NotNull("field1");
            builder.NotNull("field2");
            
            Assert.AreEqual("field1_Mod=notnull&field2_Mod=notnull", builder.Uri);
        }

        [Test]
        public void IsNull_Adds_Constraint()
        {
            var builder = new FilterBuilder();
            builder.IsNull("field1");
            builder.IsNull("field2");
            
            Assert.AreEqual("field1_Mod=notnull&field2_Mod=notnull", builder.Uri);
        }

        void ExpectCorrectDateRangeFilter(string field, string opcode, Func<string, DateTime, List<string>> operation)
        {
            var date = new DateTime(2010, 12, 15);
            string attaskDate = date.ToAtTaskDate();
            List<string> filter = operation(field, date);
            Assert.AreEqual(2, filter.Count);
            Assert.IsTrue(filter.Contains(String.Format("{0}={1}", field, attaskDate)));
            Assert.IsTrue(filter.Contains(String.Format("{0}_Mod={1}", field, opcode)));
        }
    }
}
