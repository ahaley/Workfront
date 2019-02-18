using ahaley.Workfront.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ahaley.Workfront
{
    public class DocumentDownloader
    {
        readonly string rootUrl;
        readonly string sessionID;

        public DocumentDownloader(WorkfrontConfiguration config)
        {
            this.rootUrl = config.UrlPrefix;
            this.sessionID = config.SessionID;
        }

        public void DownloadFolder(DocumentFolder[] folders)
        {
            DocumentFolder f1 = folders.First(f => f.Documents.Count() > 0);
            Document d1 = f1.Documents.First();

            var downloadUrl = string.Join("&",
                string.Join("", rootUrl, d1.DownloadURL),
                string.Join("=", "sessionID", sessionID));

            using (var client = new WebClient())
            {
                client.OpenRead(new Uri(downloadUrl));

                string disposition = client.ResponseHeaders["content-disposition"];
                var contentDisposition = new ContentDisposition(disposition);
                var fileName = ConvertFileName(contentDisposition.FileName);

                client.DownloadFile(new Uri(downloadUrl), fileName);
            }
        }

        public Action<string, int> DownloadCompleteHandler { get; set; }

        public void DownloadFile(string downloadUri, string destination)
        {
            var downloadUrl = string.Join("&",
                string.Join("", rootUrl, downloadUri),
                string.Join("=", "sessionID", sessionID));
            var uri = new Uri(downloadUrl);
            string destinationFile = string.Empty;
            using (var client = new WebClient())
            {
                Stream stream = client.OpenRead(uri);

                WebHeaderCollection headers = client.ResponseHeaders;

                IEnumerable<KeyValuePair<string, string[]>> keyValues = headers.AllKeys.Select(k => new KeyValuePair<string, string[]>(k, headers.GetValues(k)));

                string disposition = client.ResponseHeaders["content-disposition"];
                var contentDisposition = new ContentDisposition(disposition);
                var fileName = ConvertFileName(contentDisposition.FileName);
                destinationFile = Path.Combine(destination, fileName);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) =>
                {
                    DownloadCompleteHandler(downloadUri, e.ProgressPercentage);
                });
                client.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) =>
                {
                    DownloadCompleteHandler(downloadUri, 100);
                });
                client.DownloadFileAsync(uri, destinationFile);
                stream.Close();
            }
        }

        static string ConvertFileName(string fileName)
        {
            return fileName.Replace("%20", " ");
        }
    }
}
