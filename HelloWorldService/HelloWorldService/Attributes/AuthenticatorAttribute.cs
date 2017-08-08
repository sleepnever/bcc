using System;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using HelloWorldService.Helpers;

namespace HelloWorldService.Attributes
{
	/*
	 * Any class attributed with this, must pass the validation here otherwise they get a Forbidden
	 */
	public class AuthenticatorAttribute : AuthorizationFilterAttribute
	{
		public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
		{
			try
			{
				bool valid = false;

				var authorization = actionContext.Request.Headers.Authorization;
				if (authorization.Scheme == "Bearer") // <-- this is a standard HTTP schema, casing is important
				{
					var tokenString = authorization.Parameter;
					var token = TokenHelper.DecodeToken(tokenString);
					if (token.Expires > DateTime.UtcNow)
					{
						valid = true;
					}
				}

				if (!valid)
				{
					throw new HttpResponseException(HttpStatusCode.Forbidden);
				}

				return base.OnAuthorizationAsync(actionContext, cancellationToken);
			}
			catch (Exception)
			{
				throw new HttpResponseException(HttpStatusCode.Forbidden);
			}
		}
	}

}