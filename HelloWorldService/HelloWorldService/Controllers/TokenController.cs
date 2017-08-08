﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorldService.Helpers;

namespace HelloWorldService.Controllers
{
	public class TokenRequest : ApiController
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class TokenController : ApiController
	{
		// This should require SSL
		public dynamic Post([FromBody]TokenRequest tokenRequest)
		{
			var token = TokenHelper.GetToken(tokenRequest.UserName, tokenRequest.Password);
			return new { Token = token };
		}
	}
}
