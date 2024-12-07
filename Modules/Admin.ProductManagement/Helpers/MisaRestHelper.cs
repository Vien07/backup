using Admin.ProductManagement.Database;
using Admin.ProductManagement.DTOs;
using Steam.Core.Common.SteamString;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Admin.ProductManagement.Helpers
{
    public class MisaRestHelper : IMisaRestHelper
    {
        IMisaApiTrackerHelper _trackHelper;
        public MisaRestHelper(IMisaApiTrackerHelper trackHelper)
        {
            _trackHelper = trackHelper;
        }
        public MisaResponseModel<Tmodel> GET<Tmodel>(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, Dictionary<string, string>? parameters) where Tmodel : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseURL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response;

                if (parameters is not null)
                {
                    requestURL = $"{requestURL}?{HttpUtility.UrlEncode(string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")))}";

                }

                response = client.GetAsync(requestURL).Result;  // Blocking

                var dataStringReturn = response.Content.ReadAsStringAsync().Result;
                var dataReturn = JsonSerializer.Deserialize<MisaResponseModel<Tmodel>>(dataStringReturn);

                client.Dispose();

                #region save log
                MisaApiTracker tracker = new();
                tracker.Name = name;
                tracker.Method = "GET";
                tracker.ResponseStatusCode = dataReturn.Code.ToString();
                tracker.Endpoint = baseURL + requestURL;
                tracker.RequestDate = DateTime.Now;
                tracker.RequestHeaders = headers is null ? "" : headers.ToJson();
                tracker.RequestParams = parameters is null ? "" : parameters.ToJson();
                tracker.Response = dataStringReturn;

                _trackHelper.SaveLog(tracker);
                #endregion

                return dataReturn;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public MisaResponseModel<Tmodel> POST<Tmodel>(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, Dictionary<string, string>? parameters) where Tmodel : class
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseURL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response;

                if (parameters is not null)
                {
                    var jsonParam = JsonSerializer.Serialize(parameters);
                    var data = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                    response = client.PostAsync(requestURL, data).Result;  // Blocking
                }
                else
                {
                    response = client.PostAsync(requestURL, null).Result;  // Blocking
                }

                var dataStringReturn = response.Content.ReadAsStringAsync().Result;
                var dataReturn = JsonSerializer.Deserialize<MisaResponseModel<Tmodel>>(dataStringReturn);

                client.Dispose();

                #region save log
                MisaApiTracker tracker = new();
                tracker.Name = name;
                tracker.Method = "POST";
                tracker.ResponseStatusCode = dataReturn.Code.ToString();
                tracker.Endpoint = baseURL + requestURL;
                tracker.RequestDate = DateTime.Now;
                tracker.RequestHeaders = headers is null ? "" : headers.ToJson();
                tracker.RequestParams = parameters is null ? "" : parameters.ToJson();
                tracker.Response = dataStringReturn;

                _trackHelper.SaveLog(tracker);
                #endregion

                return dataReturn;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public MisaResponseModel<dynamic> POST(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, dynamic? parameters)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseURL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response;

                if (parameters is not null)
                {
                    var jsonParam = JsonSerializer.Serialize(parameters);
                    var data = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                    response = client.PostAsync(requestURL, data).Result;  // Blocking
                }
                else
                {
                    response = client.PostAsync(requestURL, null).Result;  // Blocking
                }

                var dataStringReturn = response.Content.ReadAsStringAsync().Result;
                dynamic dataReturn = JsonSerializer.Deserialize<MisaResponseModel<object>>(dataStringReturn);

                client.Dispose();

                #region save log
                MisaApiTracker tracker = new();
                tracker.Name = name;
                tracker.Method = "POST";
                tracker.ResponseStatusCode = dataReturn.Code.ToString();
                tracker.Endpoint = baseURL + requestURL;
                tracker.RequestDate = DateTime.Now;
                tracker.RequestHeaders = headers is null ? "" : headers.ToJson();
                tracker.RequestParams = parameters is null ? "" : JsonSerializer.Serialize(parameters);
                tracker.Response = dataStringReturn;

                _trackHelper.SaveLog(tracker);
                #endregion

                return dataReturn;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
