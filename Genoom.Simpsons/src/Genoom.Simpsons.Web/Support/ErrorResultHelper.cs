using System;
using System.Net;

namespace Genoom.Simpsons.Web.Support
{
    public static class ErrorResultHelper
    {
        public static ErrorResult Create(Exception exception, HttpStatusCode errorCode = HttpStatusCode.InternalServerError)
        {
            return new ErrorResult
            {
                Code = (int)errorCode,
                Message = exception.Message,
                Help = exception.HelpLink
            };
        }
    }
}
