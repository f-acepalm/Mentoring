using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadManager
{
    internal class Downloader
    {
        private readonly string _downloadDirectory;
        private bool _downloadingWasCanceled;

        public Downloader(string downloadDirectory)
        {
            _downloadDirectory = downloadDirectory;
        }

        public async Task DownloadPageAsync(Uri uri, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uri, cancellationToken))
            using (HttpContent content = response.Content)
            {
                var data = await content.ReadAsStringAsync();

                if (_downloadingWasCanceled)
                {
                    _downloadingWasCanceled = false;
                    return;
                }

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
            _downloadingWasCanceled = true;
        }

        private string GetFileName(Uri uri)
        {
            var name = uri.Segments.Last();
            return Uri.UnescapeDataString(name) + ".html";
        }
    }
}
