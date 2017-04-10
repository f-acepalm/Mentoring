using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlTools;
using System.IO;

namespace XmlToolsTests
{
    [TestClass]
    public class RssGeneratorTest
    {
        private const string Source = @"..\..\books.xml";
        private const string XsltPath = @"..\..\BooksToRSS.xslt";
        private const string ResultPath = @"..\..\RssResult.xml";

        [TestMethod]
        public void Generate_WithStreamWriter()
        {
            var generator = new RssGenerator();
            File.Delete(ResultPath);

            using (var writer = new StreamWriter(ResultPath))
            {
                generator.Generate(Source, XsltPath, writer);
            }

            Assert.IsTrue(File.Exists(ResultPath));
        }
                
        [TestMethod]
        public void Generate_WithResultPath()
        {
            var generator = new RssGenerator();
            File.Delete(ResultPath);

            generator.Generate(Source, XsltPath, ResultPath);

            Assert.IsTrue(File.Exists(ResultPath));
        }

        [TestMethod]
        public void Generate_WithConsoleWriter()
        {
            var generator = new RssGenerator();
            generator.Generate(Source, XsltPath, Console.Out);
        }
    }
}
