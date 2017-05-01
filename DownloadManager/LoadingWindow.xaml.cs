using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var initializer = new Initializer();
            var tasks = new List<Task>()
            {
                initializer.Initialize(StatusLabel1),
                initializer.Initialize(StatusLabel2),
                initializer.Initialize(StatusLabel3),
                initializer.Initialize(StatusLabel4),
                initializer.Initialize(StatusLabel5),
            };
            await Task.WhenAll(tasks);
            this.Close();
        }
    }
}
