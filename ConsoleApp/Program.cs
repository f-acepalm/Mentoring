using Castle.DynamicProxy;
using DownloadManager;
using HtmlAgilityPack;
using IanaUtilities;
using ImageProcessing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Ninject;
using Ninject.Parameters;
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
            //var generator = new ProxyGenerator();
            //var test = generator.CreateInterfaceProxyWithTarget<ITestClass>(new TestClass(), new ProxyTest());
            //var x = test.TestMethod(1, 2);

            //var x = new TestClass().TestMethod(1, 2);

            IKernel ninjectKernel = new StandardKernel(new NinjectConfigModule());
            var test = ninjectKernel.Get<ITestClass>(new Parameter("test", "valueTest", false));
            var x = test.TestMethod(1, 2);
        }
    }
}
