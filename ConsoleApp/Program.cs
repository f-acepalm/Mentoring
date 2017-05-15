using DownloadManager;
using HtmlAgilityPack;
using IanaUtilities;
using ImageProcessing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using PowerStateManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp // Этот проект создан для быстрой проверки чего-нибудь. Можно сказать, что его не существует в реальном мире.
{
    class Program
    {
        private static string queueName = @"Test4";

        static void Main(string[] args)
        {
            var nsManager = NamespaceManager.Create();
            if (!nsManager.QueueExists(queueName))
            {
                nsManager.CreateQueue(queueName);
            }

            var client = QueueClient.Create(queueName);
            client.Send(new BrokeredMessage("lol"));
            //QueueClient client = QueueClient.Create(queueName, ReceiveMode.ReceiveAndDelete);

            //var message = client.Receive();
            //var data = message.GetBody<string>();

            //client.Close();
        }
    }
}
