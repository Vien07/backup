using Microsoft.AspNetCore.Http;
using Steam.Core.Infrastructure.HttpHandle;
using Steam.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Infrastructure.Web
{
    public class ResponseSuccessfulDataHelper<T> : IApplicationResponse, IExtendableObject
    {
        public int StatusCode { get; set; }

        public string TraceId { get; set; }

        public string TranslationKey { get; set; }

        public string Message { get; private set; }

        public T Data { get; private set; }

        public string ExtensionData { get; set; }


        public ResponseSuccessfulDataHelper(T data, string translationKey = "ActionSucceed")
        {
            StatusCode = StatusCodes.Status200OK;
            TranslationKey = translationKey;
            Data = data;
        }

        public object Response() => this;

        public ResponseSuccessfulDataHelper<T> WithFriendlyMessage(string message = "Action succeed")
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            return this;
        }
        public ResponseSuccessfulDataHelper<T> SetExtensionData(string data)
        {
            ExtensionData = data;
            return this;
        }
    }
}
