using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HelloWorldService.Tests
{
    [TestClass]
    public class UnitTest1
    {
        HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/helloworldservice/api/");
        }

        [TestMethod]
        public void VerifyAddNewContact()
        {
            var newContact = new Contact
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
            var postContent = new StringContent(newJson, Encoding.UTF8, "application/json");
            var postResult = client.PostAsync("contacts", postContent).Result;
            Assert.AreEqual(HttpStatusCode.Created, postResult.StatusCode);

        }

        [TestMethod]
        public void VerifyGetAllContacts()
        {

        }

        [TestMethod]
        public void VerifyGetContactById()
        {

        }

        [TestMethod]
        public void VerifyDeleteContact()
        {

        }
    }

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
}
