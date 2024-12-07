using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface ILoggerHelper
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string nameSpace, string method, string errorMess, string data);
        void LogError(string message);
    }
}
