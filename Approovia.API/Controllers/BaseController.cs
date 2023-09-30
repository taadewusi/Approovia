using Approovia.ViewModels.Generics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Approovia.API.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public ObjectResult CustomResult(HttpStatusCode statusCode, string responseCode, string responseDescription)
        {
            responseDescription = ContainExceptionCommonWord(responseDescription) ?
                Constants.DefaultExceptionFriendlyMessage : responseDescription;

            return new CustomObjectResult(statusCode, responseCode, responseDescription?.Replace("\r\n", " | "));
        }

        [NonAction]
        public bool ContainExceptionCommonWord(string message)
        {
            if (string.IsNullOrEmpty(message))
                return false;

            var status = Constants.ExceptionCommonWords.Split(',')?.Any(message.ToLower().Contains) ?? false;

            return status;
        }


    }
}
