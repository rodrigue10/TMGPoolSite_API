using System.Text.Json.Serialization;

namespace TMG_Site_API.NodeAPI.Models
{
    public class GetBlock :IGetBlock
    {

        [JsonPropertyName("block")]
        public string Block { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("generator")]
        public string Generator { get; set; }

        [JsonPropertyName("generatorRS")]
        public string GeneratorRS { get; set; }

        [JsonPropertyName("generatorPublicKey")]
        public string GeneratorPublicKey { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("scoopNum")]
        public int ScoopNum { get; set; }

        [JsonPropertyName("timestamp")]
        public int Timestamp { get; set; }

        [JsonPropertyName("numberOfTransactions")]
        public int NumberOfTransactions { get; set; }

        [JsonPropertyName("totalAmountNQT")]
        public string TotalAmountNQT { get; set; }

        [JsonPropertyName("totalFeeNQT")]
        public string TotalFeeNQT { get; set; }

        [JsonPropertyName("totalFeeCashBackNQT")]
        public string TotalFeeCashBackNQT { get; set; }

        [JsonPropertyName("totalFeeBurntNQT")]
        public string TotalFeeBurntNQT { get; set; }

        [JsonPropertyName("blockRewardNQT")]
        public string BlockRewardNQT { get; set; }

        [JsonPropertyName("blockReward")]
        public string BlockReward { get; set; }

        [JsonPropertyName("payloadLength")]
        public int PayloadLength { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("baseTarget")]
        public string BaseTarget { get; set; }

        [JsonPropertyName("averageCommitmentNQT")]
        public string AverageCommitmentNQT { get; set; }

        [JsonPropertyName("cumulativeDifficulty")]
        public string CumulativeDifficulty { get; set; }

        [JsonPropertyName("previousBlock")]
        public string PreviousBlock { get; set; }

        [JsonPropertyName("payloadHash")]
        public string PayloadHash { get; set; }

        [JsonPropertyName("generationSignature")]
        public string GenerationSignature { get; set; }

        [JsonPropertyName("previousBlockHash")]
        public string PreviousBlockHash { get; set; }

        [JsonPropertyName("blockSignature")]
        public string BlockSignature { get; set; }

        //[JsonPropertyName("transactions")]
        //public List<string> Transactions { get; set; }

        [JsonPropertyName("requestProcessingTime")]
        public int RequestProcessingTime { get; set; }
    }
}
