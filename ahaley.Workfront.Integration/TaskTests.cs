using ahaley.Workfront.Models;
using ahaley.Workfront.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ahaley.Workfront.Integration
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void Can_Retrieve_Task()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();

            var service = WorkfrontService.Connect(config).Result;

            string taskId = "<taskID>";

            WorkfrontTask task = service.GetTask(taskId).Result;

            Assert.IsNotNull(task);
        }

        [Test]
        public void Can_Retrieve_Tasks_Of_Project()
        {
            string projectName = "<projectName>";
            var config = WorkfrontConfiguration.CreateConfiguration();
            var service = WorkfrontService.Connect(config).Result;

            var projectFilter = new FilterBuilder();
            projectFilter.Fields = Project.SalesOrderOnly;
            projectFilter.AddConstraint("name", projectName);
            var projects = service.Search<Project>(projectFilter).Result;

            Assert.AreEqual(1, projects.Length);

            var project = projects[0];

            var filterBuilder = new FilterBuilder();
            filterBuilder.Fields = WorkfrontTask.Fields;
            filterBuilder.AddConstraint("projectID", project.ID);

            WorkfrontTask[] tasks = service.Search<WorkfrontTask>(filterBuilder).Result;

            Assert.That(tasks.Length > 0);
        }
    }
}
