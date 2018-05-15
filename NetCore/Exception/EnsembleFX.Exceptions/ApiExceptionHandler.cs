
namespace EnsembleFX.Exceptions
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Net;
    using System.Net.Http;
    public class ApiExceptionHandler : ExceptionFilterAttribute, IExceptionFilter
    {

        HttpResponseMessage response;

        public override void OnException(ExceptionContext context)

        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = string.Empty;
            var exceptionType = context.Exception.GetType();

            //Check the Exception Type
            if (exceptionType == typeof(UnauthorizedAccessException)) //401 status code
            {
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(BadRequestException)) // 400 status code
            {
                status = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException)) // 404 status code
            {
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(ResourceNotModifiedException))
            {
                status = HttpStatusCode.NotModified;
            }
            else if (exceptionType == typeof(PreConditionFailedException))
            {
                status = HttpStatusCode.PreconditionFailed;
            }
            else
            {
                status = HttpStatusCode.InternalServerError; //500 status code
            }
            response = new HttpResponseMessage(status);
            context.ExceptionHandled = true;

            //The Response Message Set by the Action During Ececution
            context.HttpContext.Response.StatusCode = (int)status;
            //context.HttpContext.Response.ContentType = "application/json";

        }


    }
}
