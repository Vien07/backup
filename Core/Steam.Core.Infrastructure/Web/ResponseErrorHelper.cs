using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Steam.Core.Infrastructure.HttpHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Infrastructure.Web
{
    public class ResponseErrorHelper : IApplicationResponse
    {
        public string TraceId { get; set; }

        public string TranslationKey { get; set; }

        public int StatusCode { get; set; }

        public string RootError { get; set; }

        public string Error { get; set; }

        public Dictionary<string, string> Details { get; private set; }

        public ResponseErrorHelper(string translationKey, int statusCode)
        {
            TranslationKey = translationKey;
            StatusCode = statusCode;
        }

        public ResponseErrorHelper() { }

        public object Response() => this;

        public ResponseErrorHelper IsErrorBadRequest(string translationKey = "BadRequest")
        {
            TranslationKey = translationKey;
            StatusCode = StatusCodes.Status400BadRequest;
            return this;
        }

        public ResponseErrorHelper IsErrorUnauthorized(string translationKey = "Unauthorized")
        {
            TranslationKey = translationKey;
            StatusCode = StatusCodes.Status401Unauthorized;
            return this;
        }

        public ResponseErrorHelper IsErrorInternalServerError(string translationKey = "InternalServerError")
        {
            TranslationKey = translationKey;
            StatusCode = StatusCodes.Status500InternalServerError;
            return this;
        }

        public ResponseErrorHelper IsErrorNotFound(string translationKey = "NotFound")
        {
            TranslationKey = translationKey;
            StatusCode = StatusCodes.Status404NotFound;
            return this;
        }

        public ResponseErrorHelper IsErrorConflict(string translationKey = "Conflict")
        {
            TranslationKey = translationKey;
            StatusCode = StatusCodes.Status409Conflict;
            return this;
        }

        public ResponseErrorHelper WithFluentValidationDetails(List<ValidationFailure> validationFailures)
        {
            Details = new();
            foreach (var item in validationFailures)
            {
                if (!Details.ContainsKey(item.PropertyName))
                    Details.Add(item.PropertyName, item.ErrorMessage);
            }
            return this;
        }

        public ResponseErrorHelper WithDetails(Dictionary<string, string> details)
        {
            Details = new(details);
            return this;
        }

        public ResponseErrorHelper WithDebugMessage(string internalError)
        {
            //if (GlobalConfiguration.EnvironmentName == "Development" || GlobalConfiguration.EnvironmentName == "Staging")
            //    RootError = internalError;

            RootError = internalError;
            return this;
        }

        public ResponseErrorHelper WithFriendlyMessage(string error = "Something went wrong, please try again or contact supporters")
        {
            Error = error;
            return this;
        }
    }

}
