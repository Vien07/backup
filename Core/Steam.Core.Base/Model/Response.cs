using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Steam.Core.Base.Models
{

    public class Response<T>
    {
        public bool IsError { get; set; } 
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");

        public T Data { get; set; }

        public JsonResult Ok()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                this.Message = "Request was successful";
            }

           
            this.IsError = false;

            return new JsonResult(this)
            {
                StatusCode = 200
            };
        }
        public JsonResult BadRequest()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                this.Message = "Bad request";
            }

            this.IsError = true;

            return new JsonResult(this)
            {
                StatusCode = 400
            };
        }
        public JsonResult NotFound()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                this.Message = "Resource not found";
            }

            this.IsError = true;

            return new JsonResult(this)
            {
                StatusCode = 404
            };
        }
        public JsonResult Unauthorized()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                this.Message = "Unauthorized access";
            }

            this.IsError = true;

            return new JsonResult(this)
            {
                StatusCode = 401
            };
        }
        public JsonResult InternalServerError()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                this.Message = "An error occurred";
            }

            this.IsError = true;

            return new JsonResult(this)
            {
                StatusCode = 500
            };
        }

    }

    public class ResponseList<TModel>
    {
        public bool IsError { get; set; } = false;
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Meta { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalItem { get; set; }
        public TModel Data { get; set; }
    }

    public class Response
    {
        public bool IsError { get; set; } = false;
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

}