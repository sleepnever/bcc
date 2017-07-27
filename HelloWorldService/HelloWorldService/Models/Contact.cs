using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HelloWorldService.Models
{
    // [JsonObject(MemberSerialization.OptIn)] // 
    public class Contact
    {
        [JsonProperty("id")] // publically, we show 'id', internally it is still 'Id', so we don't break anything
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public Phone[] Phones { get; set; }
    }

    public class Phone
    {
        [JsonProperty("number", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Number { get; set; }

        // since PhoneType.Nothing = 0, we'll ignore it as it is the default value. Otherwise we can set Home = 1 to solve
        [JsonProperty("phoneType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Home = 1,
        Mobile = 2,
    }
}