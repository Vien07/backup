using Admin.ProductManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Helpers
{
    public interface IMisaRestHelper
    {
        MisaResponseModel<Tmodel> GET<Tmodel>(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, Dictionary<string, string>? parameters) where Tmodel : class;
        MisaResponseModel<Tmodel> POST<Tmodel>(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, Dictionary<string, string> parameters) where Tmodel : class;
        MisaResponseModel<object> POST(string name, string baseURL, string requestURL, Dictionary<string, string>? headers, dynamic? parameters);
    }
}
