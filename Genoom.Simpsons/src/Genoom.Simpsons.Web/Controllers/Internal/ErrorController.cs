using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Genoom.Simpsons.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet, HttpPost, HttpPut, HttpPatch, HttpDelete, HttpHead, HttpOptions]
        public HttpResponseMessage Handle404()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                ReasonPhrase = "The requested resource is not found."
            };
            return responseMessage;
        }
    }

}
