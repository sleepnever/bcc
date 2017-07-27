using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorldService.Models;

namespace HelloWorldService.Controllers
{
    public class ContactsController : ApiController
    {
        public static List<Contact> contacts = new List<Contact>();

        // GET: api/Contacts
        public IEnumerable<Contact> Get()
        {
            return contacts;
        }

        // GET: api/Contacts/5
        public Contact Get(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        // POST: api/Contacts
        public void Post([FromBody]Contact contact)
        {
            if (contact != null)
            {
                contact.Id = contacts.Count() + 1;
                contacts.Add(contact); // This will add the contact to the list
            }
        }

        // PUT: api/Contacts/5
        // PUT is when the object already exists and you want to modify the data
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
        public void Delete(int id)
        {
            if (id >= 0)
            {
                /*
                Contact contact = contacts.FirstOrDefault(c => c.Id == id);
                if (contact != null)
                {
                    contacts.Remove(contact);
                }
                */

                // better way
                contacts.RemoveAll(c => c.Id == id);
            }
        }
    }
}