using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces
{
    public interface ILoggerManager
    {
        void LogInfo(string Message);
        void LogWarning(string Message);
        void LogDebug(string Message);
        void LogError(string Message);
    }
}
