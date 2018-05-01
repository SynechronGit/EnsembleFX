using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EnsembleFX.Exceptions
{
    /// <summary>
    /// The Custom Exception Filter
    /// </summary>
    public class ApiExceptionHandlerAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext  actionExecutedContext)
        {
            //Define the Response Message
            HttpResponseMessage response;

            //Check the Exception Type
            if (actionExecutedContext.Exception is BadRequestException) response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            else if (actionExecutedContext.Exception is NotFoundException) response = new HttpResponseMessage(HttpStatusCode.NotFound);
            else
                throw actionExecutedContext.Exception; //Alow the common exception handler to handle and return 500 

            //The Response Message Set by the Action During Ececution
            var res = actionExecutedContext.Exception.Message;
            response.Content = new StringContent(res);
            response.ReasonPhrase = res;

            //Create the Error Response
            //TODO: check the final output
            actionExecutedContext.Result = new JsonResult(response.Content.ReadAsStringAsync());
           // actionExecutedContext.HttpContext.Response= response;
        }
    }
}