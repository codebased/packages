using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ap.Common.WebApi.HttpResults
{
    public class NotModified: IHttpActionResult
    {
        private readonly ApiController _controller;

        public NotModified(string content,  ApiController controller)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

           

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            Content = content;
            _controller = controller;
        }

        public string Content { get; private set; }


        public HttpRequestMessage Request
        {
            get { return _controller.Request; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            
            StreamContent streamContent = null;
            return Task.FromResult(Execute());
        }
        
        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotModified);
            response.RequestMessage = Request;
            response.Content = new StringContent(Content);
            return response;
        }
    }
}