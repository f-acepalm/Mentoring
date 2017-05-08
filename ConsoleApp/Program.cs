using DownloadManager;
using HtmlAgilityPack;
using IanaUtilities;
using ImageProcessing;
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
        private static string _outputDir = @"Output";
        private static string _inputDir = @"Images";

        static void Main(string[] args)
        {
            var path = @"D:\work\Mentoring\Mentoring\ImageJoinerService\bin\Debug\Output\ProcessedImages\image_3.png";
            var x = new PdfGenerator(_outputDir);
            x.AddImage(path);
            x.CompleteFile();
        }
    }
}
