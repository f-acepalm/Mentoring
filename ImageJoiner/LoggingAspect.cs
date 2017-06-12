using NLog;
using NLog.Config;
using NLog.Targets;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    [Serializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        private readonly string _filePath = "log.txt";

        public override void OnEntry(MethodExecutionArgs args)
        {
            string parameters = string.Empty;
            foreach (var item in args.Arguments)
            {
                parameters = $"{parameters} | {Serializer.SerializeObject(item)}";
            }

            Log($"{DateTime.Now} | CODE REWRITING | Method - {args.Method.Name}; Parameters: {parameters}");
            args.FlowBehavior = FlowBehavior.Default;
        }

        private void Log(string data)
        {
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                sw.WriteLine(data);
            }
        }
    }
}
