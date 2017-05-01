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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Downloader _downloader;
        private const string DownloadDirectory = "Download";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            _downloader = new Downloader(DownloadDirectory);
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            var loadingWindow = new LoadingWindow();
            loadingWindow.Closed += (object sender1, EventArgs e1) => { this.Visibility = Visibility.Visible; };
            loadingWindow.Show();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = UrlTexBox.Text;
            DownloadButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Visible;

            await _downloader.DownloadPageAsync(new Uri(uri));

            DownloadButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Collapsed;

            _downloader.CancelDownloading();
        }
    }
}
