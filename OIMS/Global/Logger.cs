using System;
using System.Web;
using Elmah;

namespace OIMS.Global
{
    public class Logger
    {
        /// <summary>
        /// Log error to Elmah
        /// </summary>
        public static void LogError(Exception ex, string contextualMessage = null)
        {
            if (contextualMessage != null)
            {
                // log exception with contextual information that's visible when 
                // clicking on the error in the Elmah log
                var annotatedException = new Exception(contextualMessage, ex);
                ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
            }
            else
            {
                ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
            }
        }
    }
}