using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HelloWorldService.Models
{
	/*
     * By default, all fields in a class are serialized. You have two ways to handle this. You can use the opt in method.
     * The optin property means that every property you want serialized must have a JsonProperty tag otherwise it will not be serialized.
     * The second method is to use the ignore attribute. When a field is tagged with this attribute, the field will not be serialized at all.
     *      [JsonIgnore]
     */
	/// <summary>
	/// This is the Contact Class
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
    public class Contact
    {
        [JsonProperty("id")] // publically, we show 'id', internally it is still 'Id', so we don't break anything
        public int Id { get; set; }
        public string Name { get; set; }

		/// <summary>
		/// DateTime the Contact was added
		/// </summary>
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