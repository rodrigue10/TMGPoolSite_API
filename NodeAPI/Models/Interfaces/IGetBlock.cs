namespace TMG_Site_API.NodeAPI.Models
{
    public interface IGetBlock
    {
        public string Block { get; set; }
        public int Height { get; set; }
        public string Generator { get; set; }
        public string GeneratorRS { get; set; }
        public string GeneratorPublicKey { get; set; }
        public string Nonce { get; set; }
        public int ScoopNum { get; set; }
        public int Timestamp { get; set; }
        public int NumberOfTransactions { get; set; }
        public string TotalAmountNQT { get; set; }
        public string TotalFeeNQT { get; set; }
        public string TotalFeeCashBackNQT { get; set; }
        public string TotalFeeBurntNQT { get; set; }
        public string BlockRewardNQT { get; set; }
        public string BlockReward { get; set; }
        public int PayloadLength { get; set; }
        public int Version { get; set; }
        public string BaseTarget { get; set; }
        public string AverageCommitmentNQT { get; set; }
        public string CumulativeDifficulty { get; set; }
        public string PreviousBlock { get; set; }
        public string PayloadHash { get; set; }
        public string GenerationSignature { get; set; }
        public string PreviousBlockHash { get; set; }
        public string BlockSignature { get; set; }
      //  public List<string> Transactions { get; set; }
        public int RequestProcessingTime { get; set; }

    }
}
