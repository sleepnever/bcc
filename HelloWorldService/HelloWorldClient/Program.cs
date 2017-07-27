using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HelloWorldClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5794/api/contacts/"); // DO NOT FORGET trailing slash

            HttpResponseMessage result = client.GetAsync("contacts").Result;
            string jsonResult = result.Content.ReadAsStringAsync().Result; // .Result says give it to me NOW


        }
    }
}
