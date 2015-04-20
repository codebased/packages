using Ap.Common.Models;
using System.Net;

namespace Ap.Common.WebApi
{
    public class ApiResponseBase : OperationResult
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
