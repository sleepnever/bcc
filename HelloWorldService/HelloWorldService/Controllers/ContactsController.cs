using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorldService.Models;
using Newtonsoft.Json;

namespace HelloWorldService.Controllers
{
    public class ContactsController : ApiController
    {
        public static List<Contact> contacts = new List<Contact>();

        // GET: api/Contacts
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet]
        public Contact Get(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        // POST: api/Contacts
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