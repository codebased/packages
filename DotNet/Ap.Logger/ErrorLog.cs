

using System;
using System.Web;
using Elmah;

namespace Ap.Logger
{
    public static class ErrorLog
    {
        /// <summary>
        /// Log error to Elmah
        /// </summary>
        public static void LogError(string message, Exception innerException = null)
        {
            try
            {
                var exception = innerException;

                // log error to Elmah
                if (innerException != null)
                {
                    // log exception with contextual information that's visible when 
                    // clicking on the error in the Elmah log
                    exception = new Exception(message, innerException);

                    // Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(e));
                }

                ErrorSignal.FromCurrentContext().Raise(exception, HttpContext.Current);
            }
            catch
            {
                // uh oh! just keep going
            }
        }
    }
}
