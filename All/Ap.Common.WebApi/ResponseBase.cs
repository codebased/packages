using Ap.Common.Models;
using System.Net;

namespace Ap.Common.WebApi
{
    public class ResponseBase : OperationResult
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
