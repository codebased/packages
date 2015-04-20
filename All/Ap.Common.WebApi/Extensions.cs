using System;
using System.Net;
using System.Web.Http;

namespace Ap.Common.WebApi
{
    public static class Extensions
    {
        public static TResponse ExecuteAction<TResponse>(this ApiController controller, Func<TResponse> method,
            string exceptionError)
            where TResponse : ApiResponseBase, new()
        {

            try
            {
                var response = method();
                return response;
            }
            catch (Exception exception)
            {
                // @ todo add exception and logging dependencies.
                var response = new TResponse {StatusCode = HttpStatusCode.OK, Technical = exception.ToString()};
                return response;
            }
        }
    }
}
