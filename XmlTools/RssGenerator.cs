﻿using System;
using System.IO;
using System.Xml.Xsl;

namespace XmlTools
{
    public class RssGenerator
    {
        public void Generate(string sourcePath, string xsltPath, TextWriter textWriter)
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (xsltPath == null)
            {
                throw new ArgumentNullException(nameof(xsltPath));
            }

            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var xsl = new XslCompiledTransform();
            xsl.Load(xsltPath);
            xsl.Transform(sourcePath, null, textWriter);
        }

        public void Generate(string sourcePath, string xsltPath, string resultPath)
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (xsltPath == null)
            {
                throw new ArgumentNullException(nameof(xsltPath));
            }

            if (resultPath == null)
            {
                throw new ArgumentNullException(nameof(resultPath));
            }

            var xsl = new XslCompiledTransform();
            xsl.Load(xsltPath);
            xsl.Transform(sourcePath, resultPath);
        }
    }
}
