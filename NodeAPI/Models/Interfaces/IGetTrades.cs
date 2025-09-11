namespace TMG_Site_API.NodeAPI.Models
{
    public interface IGetTrades
    {


            public List<ITrade> Trades { get; set; }
            public int NextIndex { get; set; }
            public int RequestProcessingTime { get; set; }
        
    }
}
