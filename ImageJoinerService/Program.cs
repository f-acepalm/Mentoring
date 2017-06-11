using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing;
using System.IO;
using System.Diagnostics;
using Topshelf;
using NLog.Config;
using NLog.Targets;
using NLog;
using Castle.DynamicProxy;

namespace ImageJoinerService
{
    class Program
    {
        private static string _outputDir = @"Output";
        private static string _inputDir = @"Images";

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

            var imageJoiner = new ImageJoiner(Path.Combine(currentDir, _inputDir), Path.Combine(currentDir, _outputDir), "facepalm");
            var generator = new ProxyGenerator();
            var imageJoinerProxy = generator.CreateInterfaceProxyWithTarget<IImageJoiner>(imageJoiner, new LoggingProxy());

            var logFactory = new LogFactory(logConfig);
            HostFactory.Run(
                conf => conf.Service<IImageJoiner>(
                    serv =>
                    {
                        serv.ConstructUsing(() => imageJoinerProxy);
                        serv.WhenStarted(s => s.Start());
                        serv.WhenStopped(s => s.Stop());
                    }).UseNLog(logFactory)
            );
        }
    }
}
