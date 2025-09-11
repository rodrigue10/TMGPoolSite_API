using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TMG_Site_API.Models
{
    public interface ITmgPrice
    {
        [JsonProperty(PropertyName ="epoch")]
        [JsonPropertyName("epoch")]
        public int Epoch { get; set; }

        [JsonProperty(PropertyName = "price")]
        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "blockheight")]
        [JsonPropertyName("blockheight")]
        public int BlockHeight { get; set; }

        [JsonProperty(PropertyName = "volume")]
        [JsonPropertyName("volume")]
        public double Volume { get; set; }


    }
}
