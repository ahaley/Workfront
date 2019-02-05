using ahaley.Workfront.Models;
using ahaley.Workfront.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ahaley.Workfront
{
    public class WorkfrontService
    {
        readonly WorkfrontConfiguration config;

        readonly WorkfrontUriBuilder uriBuilder;

        public static async Task<WorkfrontService> Connect(WorkfrontConfiguration config)
        {
            var login = new WorkfrontLogin();
            await login.Login(config);
            return new WorkfrontService(config);
        }

        WorkfrontService(WorkfrontConfiguration config)
        {
            this.config = config;
            uriBuilder = new WorkfrontUriBuilder(config);
        }

        public async Task<Project> GetProject(string id)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var filterBuilder = new FilterBuilder();
                filterBuilder.Fields = Project.Fields;
                string uri = uriBuilder.CreateResourceUri("project", id, filterBuilder);
                HttpResponseMessage response = await client.GetAsync(uri);
                Project project = await response.ParseScalar<Project>();
                return project;
            }
        }

        public async Task<Document[]> GetDocuments()
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(config.VersionedUrl);

                string uri = string.Join("?",
                    string.Join("/", "document", "search"),
                    string.Join("=", "apiKey", config.Token));

                HttpResponseMessage response = await client.GetAsync(uri);
                Document[] documents = await response.ParseArray<Document>();
                return documents;
            }
        }

        public async Task<DocumentFolder[]> GetDocumentFolders()
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(config.VersionedUrl);

                var filterBuilder = new FilterBuilder();

                filterBuilder.Fields = DocumentFolder.Fields;
                filterBuilder.IsNull("parentID");

                string uri = uriBuilder.CreateSearchUri("docfdr", filterBuilder);

                HttpResponseMessage response = await client.GetAsync(uri);

                string content = await response.Content.ReadAsStringAsync();

                File.WriteAllText("document_folder.json", content);

                DocumentFolder[] folders = null;// await response.ParseArray<DocumentFolder>();
                return folders;
            }
        }
    }
}

