using ahaley.Workfront.Utilities;
using ahaley.Workfront.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ahaley.Workfront.Integration
{
    [TestFixture]
    public class DocumentsTests
    {
        [Test]
        public void Can_Filter_Search()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();
            var service = WorkfrontService.Connect(config).Result;

            string id = "<groupID>";
            string docID = "<docuID>";
            FilterBuilder filterBuilder = new FilterBuilder();
            filterBuilder.Fields = new string[] { "*" };
            
            Task<Group> document = null;
            try
            {
                document = service.Get<Group>(id, filterBuilder);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            Assert.IsNotNull(document);
        }

        [Test]
        public void Can_Search_Documents()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();

            var service = WorkfrontService.Connect(config).Result;

            var documents = service.GetDocuments().Result;

            Assert.IsNotNull(documents);
        }

        [Test]
        public void Can_Search_ProjectDocuments()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();
            var service = WorkfrontService.Connect(config).Result;

            System.Diagnostics.Stopwatch w1 = new System.Diagnostics.Stopwatch();

            Project[] projects = service.GetProjectDocuments().Result;
        }

        [Test]
        public void Can_Search_DocumentFolders()
        {
            var config = WorkfrontConfiguration.CreateConfiguration();

            var service = WorkfrontService.Connect(config).Result;

            System.Diagnostics.Stopwatch w1 = new System.Diagnostics.Stopwatch();

            w1.Start();
            var folders = service.GetDocumentFolders().Result;
            w1.Stop();

            if (folders == null)
                return;

            using (StreamWriter fs = new StreamWriter("document_report.txt"))
            {
                fs.WriteLine(string.Format("Retrieval took {0} seconds.", w1.Elapsed.Seconds));

                foreach (var folder in folders)
                {
                    string prefix = folder.Name;
                    string project = folder.Project != null ? folder.Project.Name : "<unknown>";
                    foreach (var document in folder.Documents)
                    {
                        string path = string.Join("\\", project, prefix, document.Name);
                        fs.WriteLine(path);
                    }
                }
            }

            /*
            var download = new DocumentDownloader(config);
            download.DownloadFolder(folders);
            */

            //Assert.IsNotNull(folders);
        }

    }
}
