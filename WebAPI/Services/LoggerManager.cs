using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string Message)
        {
            Logger.Debug(Message);
        }

        public void LogError(string Message)
        {
            Logger.Error(Message);
        }

        public void LogInfo(string Message)
        {
            Logger.Info(Message);
        }

        public void LogWarning(string Message)
        {
            Logger.Warn(Message);
        }
    }
}
