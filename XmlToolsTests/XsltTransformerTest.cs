using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlTools;
using System.IO;

namespace XmlToolsTests
{
    [TestClass]
    public class XsltTransformerTest
    {
        private const string Source = @"..\..\books.xml";
        private const string RssXsltFile = @"..\..\BooksToRSS.xslt";
        private const string RssResultFile = @"..\..\RssResult.xml";
        private const string HtmlXsltFile = @"..\..\BooksToHtml.xslt";
        private const string HtmlResultFile = @"..\..\HtmlResult.html";

        [TestMethod]
        public void Transform_Rss_WithStreamWriter()
        {
            var generator = new XsltTransformer();
            File.Delete(RssResultFile);

            using (var writer = new StreamWriter(RssResultFile))
            {
                generator.Transform(Source, RssXsltFile, writer);
            }

            Assert.IsTrue(File.Exists(RssResultFile));
        }
                
        [TestMethod]
        public void Transform_Rss_WithRssResultFile()
        {
            var generator = new XsltTransformer();
            File.Delete(RssResultFile);

            generator.Transform(Source, RssXsltFile, RssResultFile);

            Assert.IsTrue(File.Exists(RssResultFile));
        }

        [TestMethod]
        public void Transform_Rss_WithConsoleWriter()
        {
            var generator = new XsltTransformer();
            generator.Transform(Source, RssXsltFile, Console.Out);
        }

        [TestMethod]
        public void Transform_Html()
        {
            var generator = new XsltTransformer();
            File.Delete(HtmlResultFile);

            generator.Transform(Source, HtmlXsltFile, HtmlResultFile);

            Assert.IsTrue(File.Exists(HtmlResultFile));
        }
    }
}
