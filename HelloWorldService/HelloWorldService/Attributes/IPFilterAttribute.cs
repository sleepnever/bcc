using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HelloWorldService.Attributes
{
	public class IPFilterAttribute : ActionFilterAttribute
	{
		private Stopwatch stopwatch;

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			HttpRequestMessage currentRequest = actionContext.Request;
			stopwatch = Stopwatch.StartNew();

			string ipAddress = HttpContext.Current.Request.UserHostAddress;
			if (ipAddress == "::1")
			{
				// uncomment if we really want to block
				//throw new HttpResponseException(HttpStatusCode.Forbidden);
			}

			base.OnActionExecuting(actionContext);
		}
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			stopwatch.Stop();
			long milliseconds = stopwatch.ElapsedMilliseconds;

			string controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
			Uri requestUri = actionExecutedContext.Request.RequestUri;
			HttpMethod method = actionExecutedContext.Request.Method;
			string logFileName = HttpContext.Current.Server.MapPath("~/Logger.txt");

			string message = String.Format("{0}: \r\n Uri={1}\r\n Method={2}\r\n ElapsedTimeMs={3}\r\n",
				controllerName, requestUri, method, milliseconds);

			File.AppendAllText(logFileName, message);

			base.OnActionExecuted(actionExecutedContext);
		}
	}
}