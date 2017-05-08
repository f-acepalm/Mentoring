using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class PdfGenerator
    {
        private string _outputDirectory;
        private Document _currentDocument;
        private Section _currentSection;
        private int _currentOutputFileNumber;

        public List<string> AddedImages { get; set; }

        public PdfGenerator(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            StartNewDocument();
        }

        public void AddImage(string fullPath)
        {
            var image = _currentSection.AddImage(fullPath);

            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.RelativeVertical = RelativeVertical.Page;

            image.Top = 0;
            image.Left = 0;

            image.Height = _currentDocument.DefaultPageSetup.PageHeight;
            image.Width = _currentDocument.DefaultPageSetup.PageWidth;

            _currentSection.AddPageBreak();
            AddedImages.Add(fullPath);
        }

        public void CompleteFile()
        {
            var render = new PdfDocumentRenderer();
            render.Document = _currentDocument;
            render.RenderDocument();
            render.Save(Path.Combine(_outputDirectory, $"Result{++_currentOutputFileNumber}.pdf"));
            StartNewDocument();
        }

        public void StartNewDocument()
        {
            _currentDocument = new Document();
            _currentSection = _currentDocument.AddSection();
            AddedImages = new List<string>();
        }
    }
}
