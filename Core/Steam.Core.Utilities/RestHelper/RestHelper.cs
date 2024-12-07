using RestSharp;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Steam.Core.Utilities.STeamHelper
{
    public class RestHelper : IRestHelper
    {
        private readonly int TIMEOUT = 30000;
        private static WebProxy GetWebProxy()
        {
            var proxyUrl = "http://proxy-name.companydomain.com:9090/";
            // First create a proxy object
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyUrl),
                BypassProxyOnLocal = false,
                //UseDefaultCredentials = true, // This uses: Credentials = CredentialCache.DefaultCredentials
                //*** These creds are given to the proxy server, not the web server ***
                Credentials = CredentialCache.DefaultNetworkCredentials
                //Credentials = new NetworkCredential("proxyUserName", "proxyPassword")
            };
            return proxy;
        }

        public RestHelper() { }
        public T? Get<T>(
            string requestUrl,
            IDictionary<string, string>? requestParams = null,
            IDictionary<string, string>? requestHeaders = null, bool useWebProxy = false)
        {
            try
            {
                var options = new RestClientOptions();
                options.MaxTimeout = TIMEOUT;

                if (useWebProxy)
                {
                    options.Proxy = GetWebProxy();
                }

                var client = new RestClient(options);

                if (requestHeaders is not null)
                {
                    foreach (var header in requestHeaders)
                    {
                        client.AddDefaultHeader(header.Key, header.Value);
                    }
                }

                if (requestParams is not null)
                {
                    var uriBuilder = new UriBuilder(requestUrl);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    foreach (var param in requestParams)
                    {
                        query.Add(param.Key, param.Value);
                    }
                    uriBuilder.Query = query.ToString();
                    requestUrl = uriBuilder.ToString();
                }

                var response = client.GetJson<T>(requestUrl);
                return response;
            }
            catch { return default(T); }
        }

        public T? POST<T>(
           string requestUrl,
           object payload,
           IDictionary<string, string>? requestParams = null,
           IDictionary<string, string>? requestHeaders = null,
           bool useWebProxy = false,
           bool serializeJson = false)
        {
            try
            {
                var options = new RestClientOptions();
                options.MaxTimeout = TIMEOUT;

                if (useWebProxy)
                {
                    options.Proxy = GetWebProxy();
                }

                var client = new RestClient(options);

                if (requestHeaders is not null)
                {
                    foreach (var header in requestHeaders)
                    {
                        client.AddDefaultHeader(header.Key, header.Value);
                    }
                }

                if (requestParams is not null)
                {
                    var uriBuilder = new UriBuilder(requestUrl);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    foreach (var param in requestParams)
                    {
                        query.Add(param.Key, param.Value);
                    }
                    uriBuilder.Query = query.ToString();
                    requestUrl = uriBuilder.ToString();
                }

                var request = new RestRequest(requestUrl);

                _ = serializeJson switch
                {
                    true => request.AddJsonBody(JsonConvert.SerializeObject(payload)),
                    false => request.AddJsonBody(payload)
                };

                var response = client.ExecutePost<T>(request);
                return response.Data;
            }
            catch { return default(T); }
        }
    }
}
