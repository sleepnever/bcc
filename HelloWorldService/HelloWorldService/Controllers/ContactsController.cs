using HelloWorldService.Attributes;
using HelloWorldService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelloWorldService.Controllers
{
	[ExceptionHandling] // <--- Exception attribute class above
    public class ContactsController : ApiController
    {
		// NOTE: "static" allows us to keep contacts for the life of the service
		//		 otherewise the ContactsController ctor gets called every single time
		//		 clearing it out. This is because the controller should be stateless,
		//		 where the dB should be your state. So this is a hack for now
        public static List<Contact> contacts = new List<Contact>();

        // GET: api/Contacts
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            // Try/Catch is one way to do this, but it makes for really messy code. The other way is to create an ExceptionHandling Attribute
            //try
            //{
                //int x = 1;
                //x = x / (x - 1); // if you try and write x / 0, .NET is smart enough to say this won't compile, so this is a trick
            //}
            //catch (Exception ex)
            //{
            //    // new Anonymous type
            //    var response = new
            //    {
            //        Status = "error",
            //        Message = ex.Message,
            //    };

            //    var httpResponseMessage = new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.InternalServerError, // this can't be BadRequest because there's nothing coming into Get()
            //        Content = new ObjectContent(response.GetType(), response, new System.Net.Http.Formatting.JsonMediaTypeFormatter())
            //    };

            //    throw new HttpResponseException(httpResponseMessage);

            //}

            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet]
        public Contact Get(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        // POST: api/Contacts
		/// <summary>
		/// This is a POST
		/// </summary>
		/// <param name="contact">value parameter</param>
		/// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Contact contact)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();

            if (contact == null)
            {
                responseMessage.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {

                contact.Id = contacts.Count() + 1;
                contacts.Add(contact); // This will add the contact to the list

                // serialize the string into json
                string resultJson = JsonConvert.SerializeObject(contact);

                // create your payload content
                StringContent postContent = new StringContent(resultJson, System.Text.Encoding.UTF8, "application/json");

                // return to http the payload AND the response message
                responseMessage.StatusCode = HttpStatusCode.Created;
                responseMessage.Content = postContent;
            }

            return responseMessage;
        }

        // PUT: api/Contacts/5
        // PUT is when the object already exists and you want to modify the data
        [HttpPut]
        public void Put(int id, [FromBody]Contact contact)
        {
            // return the matching contact object
            Contact existingContact = Get(id);
            if (existingContact != null)
            {
                if (!String.IsNullOrEmpty(contact.Name)) 
                {
                    existingContact.Name = contact.Name;
                }
            }
        }

        // DELETE: api/Contacts/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            Contact contact = contacts.FirstOrDefault(c => c.Id == id);
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            if (contact == null || contacts.RemoveAll(c => c.Id == id) == 0)
            {
                httpResponse.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                httpResponse.StatusCode = HttpStatusCode.OK;
            }
            return httpResponse;
        }
    }
}