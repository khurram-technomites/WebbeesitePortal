using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApp.ErrorHandling
{
    public class ErrorDetails
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }

        public ErrorDetails()
        {
        }

        public ErrorDetails(HttpStatusCode statusCode, string message, string innerMessage)
        {
            StatusCode = statusCode;
            Message = message;
            InnerMessage = innerMessage;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
