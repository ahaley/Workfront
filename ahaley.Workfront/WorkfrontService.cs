using ahaley.Workfront.Models;
using ahaley.Workfront.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
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

        public async Task<Project> GetProject(string id, FilterBuilder filterBuilder = null)
        {
            if (filterBuilder == null)
                filterBuilder = new FilterBuilder();
            if (filterBuilder.Fields == null)
                filterBuilder.Fields = Project.Fields;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                string uri = uriBuilder.CreateResourceUri("project", id, filterBuilder);
                HttpResponseMessage response = await client.GetAsync(uri);
                Project project = await response.ParseScalar<Project>();
                return project;
            }
        }

        public async Task<T> Get<T>(string id, FilterBuilder filterBuilder) where T : WorkfrontResource, new()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var instance = new T();
                string resourceToken = instance.ResourceToken;
                string uri = uriBuilder.CreateResourceUri(resourceToken, id, filterBuilder);
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceToken + ".scalar.json"), content);
                    var jObj = JObject.Parse(content);
                    JToken data = jObj["data"];
                    var record = data.ToObject<T>();
                    //T record = await response.ParseScalar<T>();
                    return record;
                }
                return null;
                
            }
        }

        public async Task<T[]> Search<T>(FilterBuilder filterBuilder) where T : WorkfrontResource, new()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var instance = new T();
                string resourceToken = instance.ResourceToken;
                string uri = uriBuilder.CreateSearchUri(resourceToken, filterBuilder);
                var response = await client.GetAsync(uri);
                T[] records = null;
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "document_subs.json"), content);

                    var jObject = JObject.Parse(content);
                    JArray data = jObject.Value<JArray>("data");
                    records = data.ToObject<T[]>();

                    //records = await response.ParseArray<T>();
                }
                return records;
            }
        }

        public async Task<Document[]> GetDocuments(FilterBuilder filterBuilder = null)
        {
            if (filterBuilder == null)
                filterBuilder = new FilterBuilder();
            filterBuilder.Limit = 200;
            filterBuilder.Fields = Document.AllFields;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                string uri = uriBuilder.CreateSearchUri("document", filterBuilder);
                HttpResponseMessage response = await client.GetAsync(uri);

                Document[] documents = null;
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText("documents.json", content);
                    JToken jObject = JObject.Parse(content);
                    JArray data = jObject.Value<JArray>("data");
                    documents = data.ToObject<Document[]>();
                    //documents = await response.ParseArray<Document>();
                }
                return documents;
            }
        }

        public async Task<Document[]> GetDocuments(HttpClient client, FilterBuilder filterBuilder = null)
        {
            if (filterBuilder == null)
                filterBuilder = new FilterBuilder();
            filterBuilder.Fields = Document.RequiredFieldsForArchiving;

            string uri = uriBuilder.CreateSearchUri("document", filterBuilder);
            HttpResponseMessage response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
            Document[] documents = await response.ParseArray<Document>();
            return documents;
        }

        public async Task<Project[]> GetProjectDocuments()
        {
            var projects = new List<Project>();
            var filterBuilder = new FilterBuilder();
            filterBuilder.Limit = 10;
            filterBuilder.Fields = Project.DocumentFields;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                string uri = uriBuilder.CreateSearchUri("project", filterBuilder);
                var response = await client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                string responseString = await response.Content.ReadAsStringAsync();

                File.WriteAllText("project_document.json", responseString);
            }

            return null;
        }

        public async Task<DocumentFolder[]> GetDocumentFolders(HttpClient client, int limit, int first)
        {
            var filterBuilder = new FilterBuilder();
            filterBuilder.Fields = DocumentFolder.Fields;
            filterBuilder.IsNull("parentID");
            filterBuilder.Limit = limit;
            filterBuilder.First = first;

            string uri = uriBuilder.CreateSearchUri("docfdr", filterBuilder);

            HttpResponseMessage response = await client.GetAsync(uri);

            DocumentFolder[] foldersPaged = null;

            if (response.IsSuccessStatusCode)
            {
                foldersPaged = await response.ParseArray<DocumentFolder>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return foldersPaged;
        }

        public async Task<DocumentFolder[]> GetDocumentFolders()
        {
            List<DocumentFolder> folders = new List<DocumentFolder>();
            bool done = false;

            var filterBuilder = new FilterBuilder();

            filterBuilder.Fields = DocumentFolder.Fields;
            filterBuilder.IsNull("parentID");

            filterBuilder.Limit = 100;
            filterBuilder.First = 0;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);

                while (!done)
                {
                    string uri = uriBuilder.CreateSearchUri("docfdr", filterBuilder);

                    HttpResponseMessage response = await client.GetAsync(uri);

                    DocumentFolder[] foldersPaged = null;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        File.AppendAllText("document_folder.json", content);
                        JObject job = JObject.Parse(content);
                        JArray jar = job.Value<JArray>("data");

                        //foldersPaged = await response.ParseArray<DocumentFolder>();
                        foldersPaged = jar.ToObject<DocumentFolder[]>();
                        folders.AddRange(foldersPaged);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    filterBuilder.First += filterBuilder.Limit;

                    done = foldersPaged.Length < filterBuilder.Limit;
                }

                return folders.ToArray();
            }
        }

        public async Task<WorkfrontTask> GetTask(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var filterBuilder = new FilterBuilder();
                filterBuilder.Fields = WorkfrontTask.Fields;
                string uri = uriBuilder.CreateResourceUri("task", id, filterBuilder);
                HttpResponseMessage response = await client.GetAsync(uri);

                WorkfrontTask task = null;
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText("task.json", content);
                    var jObject = JObject.Parse(content);
                    JToken data = jObject["data"];
                    task = data.ToObject<WorkfrontTask>();
                    //task = await response.ParseScalar<WorkfrontTask>();
                }

                return task;
            }
        }
    }
}

