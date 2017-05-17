using ImageProcessing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    public class DocumentServer
    {
        private string _outputDirectory;
        private Thread _workingThread;
        private string _queueName = "ServerQueue";
        private string _statusQueueName = "StatusQueue";
        private string _settingsQueueName = "SettingsQueue";
        private int _currentFileNumber;
        private string _statusDirectory;
        private Thread _getStatusThread;
        private FileSystemWatcher _watcher;
        private string _settingsFilePath;
        //private Thread _sendSettingsThread;
        private ManualResetEvent _workStoped;
        //private AutoResetEvent _settingsChanged;

        public DocumentServer(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            _statusDirectory = Path.Combine(outputDirectory, "Statuses");
            _settingsFilePath = Path.Combine(outputDirectory, "Settings.xml");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            if (!Directory.Exists(_statusDirectory))
            {
                Directory.CreateDirectory(_statusDirectory);
            }

            var nsManager = NamespaceManager.Create();

            if (!nsManager.QueueExists(_queueName))
            {
                nsManager.CreateQueue(_queueName);
            }

            if (!nsManager.QueueExists(_settingsQueueName))
            {
                nsManager.CreateQueue(_settingsQueueName);
            }

            _workStoped = new ManualResetEvent(false);
            //_settingsChanged = new AutoResetEvent(false);
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_settingsFilePath));
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = Path.GetFileName(_settingsFilePath); // если фильтровать, то не видит файл, а если не фильтровать, то видит
            _watcher.Changed += OnFileChanged;

            _workingThread = new Thread(WorkProcedure);
            _getStatusThread = new Thread(GetStatusProcedure);
            //_sendSettingsThread = new Thread(SendSettingsProcedure);
        }

        public void Start()
        {
            _workingThread.Start();
            _getStatusThread.Start();
            //_sendSettingsThread.Start();
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _workStoped.Set();
            _workingThread.Join();
            _getStatusThread.Join();
            //_sendSettingsThread.Join();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            var queueClient = QueueClient.Create(_settingsQueueName);
            var message = new BrokeredMessage(GetSettings());
            queueClient.Send(message);
            queueClient.Close();
        }

        //private void SendSettingsProcedure()
        //{
        //    while (WaitHandle.WaitAny(new WaitHandle[] { _workStoped, _settingsChanged }) != 0)
        //    {
        //        var queueClient = QueueClient.Create(_settingsQueueName);
        //        var message = new BrokeredMessage(GetSettings());
        //        queueClient.Send(message);
        //        queueClient.Close();
        //    }
        //}

        private ImageJoinerSettings GetSettings()
        {
            using (var fs = new FileStream(_settingsFilePath, FileMode.Open)) // а тут падает эксепшен, что не видит файл, но если скопировать ссылку из ексепшена, то файл из нее существует
            {
                var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(ImageJoinerSettings));

                return (ImageJoinerSettings)serializer.ReadObject(reader); 
            }
        }

        private void WorkProcedure()
        {
            QueueClient client = QueueClient.Create(_queueName, ReceiveMode.ReceiveAndDelete);
            while (!_workStoped.WaitOne(0))
            {
                var message = client.Receive();
                if (message != null)
                {
                    ProcessMessage(message);
                }
            }

            client.Close();
        }

        private void GetStatusProcedure()
        {
            QueueClient client = QueueClient.Create(_statusQueueName, ReceiveMode.ReceiveAndDelete);
            while (!_workStoped.WaitOne(0))
            {
                var message = client.Receive();
                if (message != null)
                {
                    ProcessStatusMessage(message);
                }
            }

            client.Close();
        }

        private void ProcessStatusMessage(BrokeredMessage message)
        {
            var data = message.GetBody<ImageJoinerStatus>();
            var serializer = new DataContractSerializer(typeof(ImageJoinerStatus));
            using (var stream = new FileStream(Path.Combine(_statusDirectory, $"{data.ServiceName}Status.xml"), FileMode.Create))
            {
                serializer.WriteObject(stream, data);
            }
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            var data = message.GetBody<byte[]>();
            File.WriteAllBytes(Path.Combine(_outputDirectory, $"Result{++_currentFileNumber}.pdf"), data);
        }
    }
}
