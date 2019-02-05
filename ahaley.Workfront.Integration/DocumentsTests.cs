using NUnit.Framework;
using System;

namespace ahaley.Workfront.Integration
{
    [TestFixture]
    public class DocumentsTests : ConfigCreator
    {
        [Test]
        public void Can_Search_Documents()
        {
            var config = CreateConfig();

            var service = WorkfrontService.Connect(config).Result;

            var documents = service.GetDocuments().Result;

            Assert.IsNotNull(documents);
        }

        [Test]
        public void Can_Search_DocumentFolders()
        {
            var config = CreateConfig();

            var service = WorkfrontService.Connect(config).Result;

            var folders = service.GetDocumentFolders().Result;

            //Assert.IsNotNull(folders);
        }
    }
}
