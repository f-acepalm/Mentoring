using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{
    internal class Downloader
    {
        private readonly string _downloadDirectory;

        public Downloader(string downloadDirectory)
        {
            _downloadDirectory = downloadDirectory;
        }

        public async Task DownloadPageAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uri))
            using (HttpContent content = response.Content)
            {
                var data = await content.ReadAsStringAsync();
                if (!Directory.Exists(_downloadDirectory))
                {
                    Directory.CreateDirectory(_downloadDirectory);
                }
                var fullPath = string.Format($"{_downloadDirectory}\\{GetFileName(uri)}");
                File.WriteAllText(fullPath, data, Encoding.Unicode);
            }
        }

        public void CancelDownloading()
        {
        }

        private string GetFileName(Uri uri)
        {
            var name = uri.Segments.Last();
            return Uri.UnescapeDataString(name) + ".html";
        }
    }
}
