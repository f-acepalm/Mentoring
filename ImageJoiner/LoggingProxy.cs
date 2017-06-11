using Castle.DynamicProxy;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;

namespace ImageProcessing
{
    public class LoggingProxy : IInterceptor
    {
        private Logger _logger;

        public LoggingProxy()
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

        public void Intercept(IInvocation invocation)
        {
            string parameters = string.Empty;
            foreach (var item in invocation.Arguments)
            {
                parameters = $"{parameters} | {Serializer.SerializeObject(item)}";
            }

            _logger.Debug($"Method - {invocation.Method.Name}; Parameters: {parameters}");
            invocation.Proceed();
        }
    }
}
