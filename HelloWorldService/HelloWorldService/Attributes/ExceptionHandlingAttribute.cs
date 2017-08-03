using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace HelloWorldService.Attributes
{
	public class ExceptionHandlingAttribute : ExceptionFilterAttribute
	{
		// ** This can also be done in Global.asax.cs, so you don't have to remember to do this on every class. Entire service is protected
		//    no matter how many controllers you add
		//    There is a commented out line in there, so that everything gets this class
		// this is basically our "catch block"
		// the 'actionExecutedContext' gives us an Exception, the Request object, the Response object (if exists), which is great for logging
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			// ignore this type because we handle it elsewhere (IPFliterAttribute, etc)
			if (actionExecutedContext.Exception.GetType() == typeof(HttpResponseException))
			{
				return;
			}

			var response = new
			{
				Status = "error",
				Message = actionExecutedContext.Exception.Message,
			};
			var httpResponseMessage = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new ObjectContent(response.GetType(), response, new System.Net.Http.Formatting.JsonMediaTypeFormatter())
			};
			throw new HttpResponseException(httpResponseMessage);
		}
	}
}