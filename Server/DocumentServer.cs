using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class DocumentServer
    {
        private string _outputDirectory;
        private Thread _workingThread;
        private bool _isworkStoped;
        private string _queueName = "ServerQueue";

        public DocumentServer(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            var nsManager = NamespaceManager.Create();

            if (!nsManager.QueueExists(_queueName))
            {
                nsManager.CreateQueue(_queueName);
            }

            _workingThread = new Thread(WorkProcedure);
        }

        public void Start()
        {
            _isworkStoped = false;
            _workingThread.Start();
        }

        public void Stop()
        {
            _isworkStoped = true;
            _workingThread.Join();
        }

        private void WorkProcedure()
        {
            QueueClient client = QueueClient.Create(_queueName, ReceiveMode.ReceiveAndDelete);
            while (!_isworkStoped)
            {
                var message = client.Receive();
                if (message != null)
                {
                    ProcessMessage(message);
                }
            }

            client.Close();
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            var data = message.GetBody<string>();
            Console.WriteLine("{0} - {1}", message.MessageId, data);
        }
    }
}
