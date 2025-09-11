using System.Text.Json.Serialization;

namespace TMG_Site_API.NodeAPI.Models
{
    public class GetTrades
    {
        [JsonPropertyName("trades")]
        public List<Trade> Trades { get; set; }

        [JsonPropertyName("nextIndex")]
        public int NextIndex { get; set; }

        [JsonPropertyName("requestProcessingTime")]
        public int RequestProcessingTime { get; set; }
    }
}
