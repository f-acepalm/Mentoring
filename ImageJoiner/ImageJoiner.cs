using Castle.DynamicProxy;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImageJoiner : IImageJoiner
    {
        private FileSystemWatcher _watcher;
        private string _inputDirectory;
        private string _outputDirectory;
        private string _processedImagesDirectory;
        private string _brokenFilesDirectory;
        private Thread _workingThread;
        private ManualResetEvent _workStoped;
        private AutoResetEvent _newFileAdded;
        private IPdfGenerator _pdfGenerator;
        private int _previousFileNumber = -1;
        private Thread _updateStatusThread;
        private string _settingsQueueName = "SettingsQueue";
        private string _statusQueueName = "StatusQueue";
        private int _statusUpdateTimeout = 5000;
        private string _name;
        private Thread _getSettingsThread;

        public ImageJoiner(string inputDirectory, string outputDirectory, string name)
        {
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
            _processedImagesDirectory = Path.Combine(outputDirectory, "ProcessedImages");
            _brokenFilesDirectory = Path.Combine(outputDirectory, "BrokenImages");
            _name = name;

            if (!Directory.Exists(inputDirectory))
            {
                Directory.CreateDirectory(inputDirectory);
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            if (!Directory.Exists(_processedImagesDirectory))
            {
                Directory.CreateDirectory(_processedImagesDirectory);
            }

            if (!Directory.Exists(_brokenFilesDirectory))
            {
                Directory.CreateDirectory(_brokenFilesDirectory);
            }

            var nsManager = NamespaceManager.Create();
            if (!nsManager.QueueExists(_statusQueueName))
            {
                nsManager.CreateQueue(_statusQueueName);
            }

            //_pdfGenerator = new PdfGenerator(outputDirectory);
            var generator = new ProxyGenerator();
            _pdfGenerator = generator.CreateInterfaceProxyWithTarget<IPdfGenerator>(new PdfGenerator(outputDirectory), new LoggingProxy());


            _workingThread = new Thread(WorkProcedure);
            _updateStatusThread = new Thread(UpdateStatusProcedure);
            _getSettingsThread = new Thread(GetSettingsProcedure);
            _workStoped = new ManualResetEvent(false);
            _newFileAdded = new AutoResetEvent(false);

            _watcher = new FileSystemWatcher(inputDirectory);
            _watcher.Created += OnFileCreated;
        }

        //[LoggingAspect]
        public void Start()
        {
            _workingThread.Start();
            _updateStatusThread.Start();
            _getSettingsThread.Start();
            _watcher.EnableRaisingEvents = true;
        }

        //[LoggingAspect]
        public void Stop()
        {
            try
            {
                _watcher.EnableRaisingEvents = false;
                _workStoped.Set();
                _pdfGenerator.CompleteFileWithQueue();
            }
            catch (OutOfMemoryException)
            {
                MoveBrokenFiles(_pdfGenerator.AddedImages);
                _pdfGenerator.StartNewDocument();
            }
            finally
            {
                _workingThread.Join();
                _updateStatusThread.Join();
                _getSettingsThread.Join();
            }
        }

        private void MoveBrokenFiles(List<string> addedImages)
        {
            foreach (var filePath in addedImages)
            {
                MoveFile(filePath, Path.Combine(_brokenFilesDirectory, Path.GetFileName(filePath)));
            }
        }

        private void GetSettingsProcedure()
        {
            QueueClient client = QueueClient.Create(_settingsQueueName, ReceiveMode.ReceiveAndDelete);
            while (!_workStoped.WaitOne(0))
            {
                var message = client.Receive();
                if (message != null)
                {
                    ProcessSettingsMessage(message);
                }
            }

            client.Close();
        }

        private void ProcessSettingsMessage(BrokeredMessage message)
        {
            var data = message.GetBody<ImageJoinerSettings>();
            _statusUpdateTimeout = data.UpdateStatusTimeout;
        }

        private void UpdateStatusProcedure()
        {
            var queueClient = QueueClient.Create(_statusQueueName);
            do
            {
                var message = new BrokeredMessage(new ImageJoinerStatus
                {
                    ServiceName = _name,
                    LastFileNumber = _previousFileNumber,
                    CurrentDate = DateTime.Now,
                    UpdateStatusTimeout = _statusUpdateTimeout
                });
                queueClient.Send(message);
                Thread.Sleep(_statusUpdateTimeout);
            }
            while (!_workStoped.WaitOne(0));

            queueClient.Close();
        }

        private void WorkProcedure()
        {
            do
            {
                foreach (var filePath in Directory.EnumerateFiles(_inputDirectory))
                {
                    if (_workStoped.WaitOne(TimeSpan.Zero))
                    {
                        return;
                    }

                    if (TryToOpen(filePath, 3) && IsFormatRight(filePath))
                    {
                        var newFilePath = Path.Combine(_processedImagesDirectory, Path.GetFileName(filePath));
                        MoveFile(filePath, newFilePath);
                        ProcessImage(newFilePath);
                    }
                }
            }
            while (WaitHandle.WaitAny(new WaitHandle[] { _workStoped, _newFileAdded }) != 0);
        }

        private static void MoveFile(string fileName, string newFilePath)
        {
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(fileName, newFilePath);
        }

        private bool IsFormatRight(string fileName)
        {
            var pattern = @"image_\d+[.](?:png|jpeg|jpg)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(fileName);
        }

        private void ProcessImage(string filePath)
        {
            var pattern = @"\d+";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var fileNumber = int.Parse(regex.Match(Path.GetFileName(filePath)).ToString());
            try
            {
                if (_previousFileNumber >= 0 && fileNumber - _previousFileNumber != 1)
                {
                    _pdfGenerator.CompleteFileWithQueue();
                }
            }
            catch (OutOfMemoryException)
            {
                MoveBrokenFiles(_pdfGenerator.AddedImages);
                _pdfGenerator.StartNewDocument();
            }
            finally
            {
                _previousFileNumber = fileNumber;
                _pdfGenerator.AddImage(filePath);
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            _newFileAdded.Set();
        }

        private bool TryToOpen(string fullPath, int attemptCount)
        {
            for (int i = 0; i < attemptCount; i++)
            {
                try
                {
                    var file = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    file.Close();

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(3000);
                }
            }

            return false;
        }
    }
}
