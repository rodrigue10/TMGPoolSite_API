using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TMG_Site_API.Models
{
    public class ErrorData
    {

        [JsonProperty(PropertyName = "errorCode")]
        [JsonPropertyName("errorCode")]
        public int errorCode { get; set;}


        [JsonProperty(PropertyName = "errorDescription")]
        [JsonPropertyName("errorDescription")]
        public string errorDescription { get; set; }
    }
}
