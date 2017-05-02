using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private const string DownloadDirectory = "Download";
        private readonly Downloader _downloader;
        private CancellationTokenSource _cancellationTokenSource;

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
            _cancellationTokenSource = new CancellationTokenSource();
            var uri = UrlTexBox.Text;
            try
            {
                DownloadButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Visible;
                RunProgressBar();
                await _downloader.DownloadPageAsync(new Uri(uri), _cancellationTokenSource.Token);
                StopProgressBar();
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                DownloadButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
        }

        private void StopProgressBar()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void RunProgressBar()
        {
            for (int i = 0; i < 100; i++)
            {
                ProgressBar.Value++;
                await Task.Delay(50);
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    ProgressBar.Value = 0;
                    break;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}
