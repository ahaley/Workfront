using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ahaley.Workfront.Integration
{
    [TestFixture]
    public class LoginTests
    {
        [Test]
        public void Can_Retrieve_Project()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();

            WorkfrontService service = null;

            try {
                service = WorkfrontService.Connect(config).Result;
            }
            catch (Exception ex) {
                Assert.Fail(ex.Message);
            }

            Assert.IsNotNull(service);
        }
    }
}
