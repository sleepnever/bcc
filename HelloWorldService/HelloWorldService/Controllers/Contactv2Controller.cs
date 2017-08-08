using System.Collections.Generic;
using System.Web.Http;

namespace HelloWorldService.Controllers
{
	[RoutePrefix("api/v2/contacts")]
    public class Contactv2Controller : ApiController
    {

		public IEnumerable<Contact2> Get()
		{
			return new Contact2[] {	new Contact2 { Id=5, Name="steve", Age=21 }	};
		}

	}

	public class Contact2
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}
}
