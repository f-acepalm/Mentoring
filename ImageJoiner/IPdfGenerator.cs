using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public interface IPdfGenerator
    {
        List<string> AddedImages { get; set; }

        void AddImage(string fullPath);

        void CompleteFile();

        void CompleteFileWithQueue();

        void StartNewDocument();
    }
}
