using System.Text.Json.Serialization;

namespace TMG_Site_API.NodeAPI.Models
{
    public class GetAT : IGetAT
    {
        [JsonPropertyName("at")]
        public string AT { get; set; }

        [JsonPropertyName("machineData")]
        public string MachineData { get; set; }

        [JsonPropertyName("balanceNQT")]
        public string BalanceNQT { get; set; }

        [JsonPropertyName("prevBalanceNQT")]
        public string PrevBalanceNQT { get; set; }

        [JsonPropertyName("nextBlock")]
        public int NextBlock { get; set; }

        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }

        [JsonPropertyName("running")]
        public bool Running { get; set; }

        [JsonPropertyName("stopped")]
        public bool Stopped { get; set; }

        [JsonPropertyName("finished")]
        public bool Finished { get; set; }

        [JsonPropertyName("dead")]
        public bool Dead { get; set; }

        [JsonPropertyName("machineCodeHashId")]
        public string MachineCodeHashId { get; set; }

        [JsonPropertyName("atVersion")]
        public int AtVersion { get; set; }

        [JsonPropertyName("atRS")]
        public string AtRS { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("creator")]
        public string Creator { get; set; }

        [JsonPropertyName("creatorRS")]
        public string CreatorRS { get; set; }

        [JsonPropertyName("minActivation")]
        public string MinActivation { get; set; }

        [JsonPropertyName("creationBlock")]
        public int CreationBlock { get; set; }

        [JsonPropertyName("machineCode")]
        public string MachineCode { get; set; }

        [JsonPropertyName("creationMachineData")]
        public string CreationMachineData { get; set; }

        [JsonPropertyName("requestProcessingTime")]
        public int RequestProcessingTime { get; set; }
    }
}
