using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using Ap.Common.WebApi.HttpResults;

namespace Ap.Common.WebApi
{
    public static class Extensions
    {
        public static TResponse ExecuteAction<TResponse>(this ApiController controller, Func<TResponse> method,
            string exceptionError)
            where TResponse : ResponseBase, new()
        {
            try
            {
                var response = method();
                return response;
            }
            catch (Exception exception)
            {
                // @ todo add exception and logging dependencies.
                var response = new TResponse { StatusCode = HttpStatusCode.OK, Technical = exception.ToString() };
                return response;
            }
        }

        public static OkTextPlainResult Text(this ApiController controller, string content)
        {
            return Text(controller, content, Encoding.UTF8);
        }

        public static OkTextPlainResult Text(this ApiController controller, string content, Encoding encoding)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            return new OkTextPlainResult(content, encoding, controller);
        }

        public static OkFileDownloadResult Download(this ApiController controller, string path, string contentType)
        {
            string downloadFileName = Path.GetFileName(path);
            return Download(controller, path, contentType, downloadFileName);
        }

        public static OkFileDownloadResult Download(this ApiController controller, string path, string contentType, string downloadFileName)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            return new OkFileDownloadResult(path, contentType, downloadFileName, controller);
        }

        public static OkFileDownloadResult Download(this ApiController controller, byte[] contents, string contentType, string downloadFileName)
        {

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            return new OkFileDownloadResult(contents, contentType, downloadFileName, controller);
        }
    }
}
