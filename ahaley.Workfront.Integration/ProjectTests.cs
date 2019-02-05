using NUnit.Framework;
using System;
using ahaley.Workfront;
using ahaley.Workfront.Models;

namespace ahaley.Workfront.Integration
{


    [TestFixture]
    public class ProjectTests : ConfigCreator
    {
     
        [Test]
        public void Can_Retrieve_Project()
        {
            var config = CreateConfig();

            var service = WorkfrontService.Connect(config).Result;

            Project project = service.GetProject("5b100909000ef303c4be94de7a4ffd82").Result;

            Assert.IsNotNull(project);
        }
    }
}
