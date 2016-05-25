using Newtonsoft.Json;

namespace Aegis.Monitor.Core
{
    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_name")]
        public string CountryName { get; set; }
    }
}