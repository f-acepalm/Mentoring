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

        static void Main(string[] args)
        {
            QueueClient client = QueueClient.Create(queueName, ReceiveMode.ReceiveAndDelete);
            var message = client.Receive();
            var data = message.GetBody<byte[]>();

            File.WriteAllBytes(@"D:\work\Mentoring\Mentoring\ImageJoinerService\bin\Debug\Output\test.pdf", data);

            client.Close();

            //var generator = new PdfGenerator("test");
            //generator.AddImage(@"D:\work\Mentoring\Mentoring\ImageJoinerService\bin\Debug\Output\ProcessedImages\image_0.jpg");

            //var stream = new MemoryStream();
            //var render = new PdfDocumentRenderer();
            //render.Document = generator.CurrentDocument;
            //render.RenderDocument();
            //render.Save(stream, false);

            //var queueClient = QueueClient.Create(queueName);
            //var message = new BrokeredMessage(stream.ToArray());

            //queueClient.Send(message);
            //queueClient.Close();
        }
    }
}
