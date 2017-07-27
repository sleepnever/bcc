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
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5794/api/"); // DO NOT FORGET trailing slash

            HttpResponseMessage responseResult = client.GetAsync("contacts").Result;
            string jsonResult = responseResult.Content.ReadAsStringAsync().Result; // .Result says give it to me NOW

            Console.WriteLine(jsonResult);

            List<dynamic> result = JsonConvert.DeserializeObject<List<dynamic>>(jsonResult);

            Console.WriteLine(result);

            Console.Write("Press Any Key to Exit...");
            Console.ReadLine();
        }
    }
}
