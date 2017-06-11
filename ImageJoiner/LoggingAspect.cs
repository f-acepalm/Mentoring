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
        [NonSerialized]
        private Logger _logger;

        public LoggingAspect()
        {
            var currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var logConfig = new LoggingConfiguration();
            var target = new FileTarget()
            {
                Name = "Def",
                FileName = Path.Combine(currentDir, "log.txt"),
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);

            var logFactory = new LogFactory(logConfig);
            _logger = logFactory.GetCurrentClassLogger();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            string parameters = string.Empty;
            foreach (var item in args.Arguments)
            {
                parameters = $"{parameters} | {Serializer.SerializeObject(item)}";
            }

            _logger.Debug($"Method - {args.Method.Name}; Parameters: {parameters}");
            args.FlowBehavior = FlowBehavior.Default;
        }
    }
}
