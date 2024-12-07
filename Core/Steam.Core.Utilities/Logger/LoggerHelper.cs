using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Steam.Core.Utilities.STeamHelper
{
    public class LoggerHelper : ILoggerHelper
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerHelper()
        {
            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\logger-manager");

            NLog.GlobalDiagnosticsContext.Set("AppDirectory", filePath);
            NLog.Common.InternalLogger.LogFile = Path.Combine(filePath, "nlog-internal.log");
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string nameSpace, string method, string errorMess, string data)
        {
            var message = string.Format("{0}.{1}()|ErrorMess:{2}|Data:{3}", nameSpace, method, errorMess, data);
            logger.Error(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }


        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

    }
}
