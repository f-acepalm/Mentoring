using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DownloadManager
{
    internal class Initializer
    {
        private Random _random = new Random();

        public async Task Initialize(ContentControl control)
        {
            await Task.Delay(1000 + _random.Next(5000));
            control.Content = "Done";
        }
    }
}
