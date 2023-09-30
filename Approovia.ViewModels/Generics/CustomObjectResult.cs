using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Approovia.ViewModels.Generics
{
    public class CustomObjectResult : ObjectResult
    {
        private static object CreateResponse(string responseCode, string responseDescription)
        {
            return new { code = responseCode, description = responseDescription, responseCode, responseDescription };
        }

        public CustomObjectResult(HttpStatusCode statusCode, string responseCode, string responseDescription) : base(CreateResponse(responseCode, responseDescription))
        {
            StatusCode = (int)statusCode;
        }

        public CustomObjectResult(HttpStatusCode statusCode, object obj) : base(obj)
        {
            StatusCode = (int)statusCode;
        }
    }
}
