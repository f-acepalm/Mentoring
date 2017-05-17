using DownloadManager;
using HtmlAgilityPack;
using IanaUtilities;
using ImageProcessing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PowerStateManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp // Этот проект создан для быстрой проверки чего-нибудь. Можно сказать, что его не существует в реальном мире.
{
    class Program
    {
        private static string queueName = @"ServerQueue";
        private static string _statusQueueName = "StatusQueue";

        static void Main(string[] args)
        {
            //var queueClient = QueueClient.Create(_statusQueueName);
            //var message = new BrokeredMessage(new ImageJoinerStatus { LastFileNumber = 10, ServiceName = "test" });

            //queueClient.Send(message);
            //queueClient.Close();


            //QueueClient client = QueueClient.Create(_statusQueueName, ReceiveMode.ReceiveAndDelete);
            //message = client.Receive();
            //var data = message.GetBody<ImageJoinerStatus>();

            //var ser = new DataContractSerializer(typeof(ImageJoinerStatus));
            //using (var stream = new FileStream($"{data.ServiceName}Settings.xml", FileMode.Create))
            //{
            //    ser.WriteObject(stream, data); 
            //}

            //client.Close();     


            var ser = new DataContractSerializer(typeof(ImageJoinerSettings));
            using (var stream = new FileStream($"Settingsssss.xml", FileMode.Create))
            {
                ser.WriteObject(stream, new ImageJoinerSettings() { UpdateStatusTimeout = 40 });
            }
        }
    }
}
