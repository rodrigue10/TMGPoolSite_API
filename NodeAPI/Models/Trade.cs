using System.Text.Json.Serialization;

namespace TMG_Site_API.NodeAPI.Models
{
    public class Trade
    {
        [JsonPropertyName("timestamp")]
        public int Timestamp { get; set; }

        [JsonPropertyName("quantityQNT")]
        public string QuantityQNT { get; set; }

        [JsonPropertyName("priceNQT")]
        public string PriceNQT { get; set; }

        [JsonPropertyName("asset")]
        public string Asset { get; set; }

        [JsonPropertyName("askOrder")]
        public string AskOrder { get; set; }

        [JsonPropertyName("bidOrder")]
        public string BidOrder { get; set; }

        [JsonPropertyName("askOrderHeight")]
        public int AskOrderHeight { get; set; }

        [JsonPropertyName("seller")]
        public string Seller { get; set; }

        [JsonPropertyName("sellerRS")]
        public string SellerRS { get; set; }

        [JsonPropertyName("buyer")]
        public string Buyer { get; set; }

        [JsonPropertyName("buyerRS")]
        public string BuyerRS { get; set; }

        [JsonPropertyName("block")]
        public string Block { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("tradeType")]
        public string TradeType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}
