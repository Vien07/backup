using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface IRestHelper
    {
        T? Get<T>(string requestUrl, IDictionary<string, string>? requestParams = null, IDictionary<string, string>? requestHeaders = null, bool useWebProxy = false);

        T? POST<T>(string requestUrl, object payload, IDictionary<string, string>? requestParams = null, IDictionary<string, string>? requestHeaders = null, bool useWebProxy = false, bool serializeJson = false);
    }
}
