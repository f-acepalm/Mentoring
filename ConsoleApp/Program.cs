using HtmlAgilityPack;
using IanaUtilities;
using PowerStateManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp // Этот проект создан для быстрой проверки чего-нибудь. Можно сказать, что его не существует в реальном мире.
{
    class Program
    {
        private static string _address = @"https://www.iana.org/domains/root/db";

        static void Main(string[] args)
        {
            //HttpClient client = new HttpClient();
            //var x = client.GetAsync(_address);
            //x.Wait();
            //HttpResponseMessage response = x.Result;
            //response.EnsureSuccessStatusCode();
            //var y = response.Content.ReadAsStringAsync();
            //y.Wait();
            //var html = y.Result;

            //HtmlDocument hap = new HtmlDocument();
            //hap.LoadHtml(html);
            //HtmlNodeCollection nodes = hap.DocumentNode.SelectNodes("//table[@id='tld-table']//span[@class='domain tld']/a");
            //if (nodes != null)
            //    foreach (HtmlNode node in nodes)
            //        Console.WriteLine(node.GetAttributeValue("href", null));

            
        }
    }
}
