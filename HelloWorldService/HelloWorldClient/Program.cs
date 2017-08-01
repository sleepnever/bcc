using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace HelloWorldClient
{
    class Program
    {
        public static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            
            client.BaseAddress = new Uri("http://localhost:5794/api/"); // DO NOT FORGET trailing slash

            // this will cause our client to POST to the service
            Contact newContact = new Contact
            {
                Name = "New Name",
                Phones = new[] {
                    new Phone {
                        Number = "425-111-2222",
                        PhoneType = PhoneType.Mobile
                    }
                }
            };

            var newJson = JsonConvert.SerializeObject(newContact);
            /*
             * Why use StringContent? Good for adding the content on HttpResponseMessage Object
                Ex: response.Content = new StringContent("Place response text here");
             */
            StringContent postContent = new StringContent(newJson, Encoding.UTF8, "application/json");
            HttpResponseMessage postResult = client.PostAsync("contacts", postContent).Result;

            // GET values from the post back and read it into a custom ResponseObject
            string postResultJson = postResult.Content.ReadAsStringAsync().Result;
            PostResponseObject responseObj = JsonConvert.DeserializeObject<PostResponseObject>(postResultJson);

            HttpResponseMessage responseResult = client.GetAsync("contacts").Result;
            string jsonResult = responseResult.Content.ReadAsStringAsync().Result; // .Result says give it to me NOW

            Console.WriteLine(jsonResult);

            // this was List<dynamic>, but now that we have the dupe'd classes locally, we can talk directly to Contact
            List<Contact> result = JsonConvert.DeserializeObject<List<Contact>>(jsonResult);

            foreach (Contact contact in result)
            {
                Console.WriteLine("Contact Name: {0}", contact.Name);
            }

            // DELETE object
            HttpResponseMessage deleteResult0 = DoRequest(HttpMethod.Delete, String.Format("contacts/{0}", responseObj.Id), null); // example for method below to clenup code
            HttpResponseMessage deleteResult = client.DeleteAsync(String.Format("contacts/{0}", responseObj.Id)).Result;
            Console.WriteLine(deleteResult.StatusCode);

            Console.Write("Press Any Key to Exit...");
            Console.ReadLine();
        }

        // this method can be called like for DELETE, as a way to save on code above
        static HttpResponseMessage DoRequest(HttpMethod httpMethod, string uri, HttpContent content = null)
        {
            
            // SendAsync vs Get/PostAsync
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(uri),
                Content = content
            };

            return client.SendAsync(requestMessage).Result;

        }
    }

    /*
     * Instead of referencing the API DLLs directly in the project, we copy what is in the service model 
     * and duplicate code and place it in our client. That way you are isolated from the service which is
     * running in production vs test vs whatever and you can modify things locally that the service doesn't
     * have.
     *      -- I don't see how this is a good idea, duplicating code. This is where having a DLL like SOS -> MVP Team
     */
    public class Contact
    {
        [JsonProperty("id")] // publically, we show 'id', internally it is still 'Id', so we don't break anything
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; }
        public Phone[] Phones { get; set; }
    }

    public class Phone
    {
        [JsonProperty("number", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Number { get; set; }

        // since PhoneType.Nothing = 0, we'll ignore it as it is the default value. Otherwise we can set Home = 1 to solve
        [JsonProperty("phone_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Home = 1,
        Mobile = 2,
    }

    // gets the post object, and then you can map and massage the data into the real object
    public class PostResponseObject
    {
        public string Id { get; set; }
        //public string SomeField { get; set; }
    }
}
