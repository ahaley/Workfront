using NUnit.Framework;
using System;
using ahaley.Workfront;
using ahaley.Workfront.Models;
using ahaley.Workfront.Utilities;
using System.Diagnostics;

namespace ahaley.Workfront.Integration
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void Can_Retrieve_Project()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();

            var service = WorkfrontService.Connect(config).Result;

            string projectId = "<projectID>";

            Project project = service.GetProject(projectId).Result;

            Assert.IsNotNull(project);
        }

        [Test]
        public void Can_Search_Project_By_Name()
        {
            string name = "<projectName>";

            var config = WorkfrontConfiguration.CreateConfiguration();
            var service = WorkfrontService.Connect(config).Result;

            var filterBuilder = new FilterBuilder();
            filterBuilder.AddConstraint("name", name);
            Project[] projects = null;
            try
            {
                projects = service.Search<Project>(filterBuilder).Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Assert.IsNotNull(projects);
        }
    }
}
