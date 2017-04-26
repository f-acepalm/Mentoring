﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IanaUtilities
{
    internal class DataProvider
    {
        private const string SiteUri = @"https://www.iana.org/domains/root/db";

        public async Task<IEnumerable<string>> GetAllDomainsAsync(HttpClient client)
        {
            using (HttpResponseMessage response = await client.GetAsync(SiteUri))
            using (HttpContent content = response.Content)
            {
                var html = await content.ReadAsStringAsync();
                return ProcessAllDomainsPage(html);
            }
        }

        public async Task<string> GetWHOISServerNameAsync(string domainName, HttpClient client)
        {
            IdnMapping idn = new IdnMapping();
            var uri = $"{SiteUri}/{idn.GetAscii(domainName)}.html";
            using (HttpResponseMessage response = await client.GetAsync(uri))
            using (HttpContent content = response.Content)
            {
                var html = await content.ReadAsStringAsync();
                return ProcessDomain(html);
            }
        }

        public IEnumerable<string> GetAllDomains(HttpClient client)
        {
            using (HttpResponseMessage response = client.GetAsync(SiteUri).Result)
            using (HttpContent content = response.Content)
            {
                var html = content.ReadAsStringAsync().Result;
                return ProcessAllDomainsPage(html);
            }
        }

        public string GetWHOISServerName(string domainName, HttpClient client)
        {
            IdnMapping idn = new IdnMapping();
            var uri = $"{SiteUri}/{idn.GetAscii(domainName)}.html";
            using (HttpResponseMessage response = client.GetAsync(uri).Result)
            using (HttpContent content = response.Content)
            {
                var html = content.ReadAsStringAsync().Result;
                return ProcessDomain(html);
            }
        }

        private static string ProcessDomain(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//p[b[text()='WHOIS Server:']]");
            if (nodes != null && nodes.Any())
            {
                return nodes.First().LastChild.InnerText.Trim();
            }

            return null;
        }

        private static IEnumerable<string> ProcessAllDomainsPage(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//table[@id='tld-table']//span[@class='domain tld']/a");
            if (nodes == null)
            {
                throw new HttpRequestException();
            }

            return nodes.Select(node => node.InnerText.Substring(1));
        }
    }
}
