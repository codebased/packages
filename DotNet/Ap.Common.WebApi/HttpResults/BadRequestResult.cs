using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Ap.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ap.Common.WebApi.HttpResults
{
    public class BadRequestResult : IHttpActionResult
    {
        private readonly ApiController _controller;

        public BadRequestResult(OperationResult operationResult, ApiController controller)
        {
            if (operationResult == null)
            {
                throw new ArgumentNullException("operationResult");
            }

            OperationResult = operationResult;
            _controller = controller;
        }

        public OperationResult OperationResult { get; private set; }

        public HttpRequestMessage Request
        {
            get { return _controller.Request; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var value = JsonConvert.SerializeObject(OperationResult, jsonSerializerSettings);

            var contents = new StringContent(value, Encoding.UTF8, "application/json");

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.RequestMessage = Request;
            response.Content = contents;
            return response;
        }
    }
}