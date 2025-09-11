using System.Text.Json.Serialization;

namespace TMG_Site_API.NodeAPI.Models
{
    public interface IGetAT
    {
        public string AT { get; set; }
        public string MachineData { get; set; }
        public string BalanceNQT { get; set; }
        public string PrevBalanceNQT { get; set; }
        public int NextBlock { get; set; }
        public bool Frozen { get; set; }
        public bool Running { get; set; }
        public bool Stopped { get; set; }
        public bool Finished { get; set; }
        public bool Dead { get; set; }
        public string MachineCodeHashId { get; set; }
        public int AtVersion { get; set; }
        public string AtRS { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string CreatorRS { get; set; }
        public string MinActivation { get; set; }
        public int CreationBlock { get; set; }
        public string MachineCode { get; set; }
        public string CreationMachineData { get; set; }
        public int RequestProcessingTime { get; set; }

    }
}
