using NLog;
using NLog.Config;
using NLog.Targets;
using Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ServerService
{
    class Program
    {
        private static string _outputDir = @"Output";

        static void Main(string[] args)
        {
            var currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var logConfig = new LoggingConfiguration();
            var target = new FileTarget()
            {
                Name = "Def",
                FileName = Path.Combine(currentDir, "log.txt"),
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);

            var logFactory = new LogFactory(logConfig);
            HostFactory.Run(
                conf => conf.Service<DocumentServer>(
                    serv =>
                    {
                        serv.ConstructUsing(() => new DocumentServer(Path.Combine(currentDir, _outputDir)));
                        serv.WhenStarted(s => s.Start());
                        serv.WhenStopped(s => s.Stop());
                    }).UseNLog(logFactory)
            );
        }
    }
}
