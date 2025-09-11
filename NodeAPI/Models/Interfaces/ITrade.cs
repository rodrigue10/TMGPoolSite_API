namespace TMG_Site_API.NodeAPI.Models
{
    public interface ITrade
    {


      
            public int Timestamp { get; set; }
            public string QuantityQNT { get; set; }
            public string PriceNQT { get; set; }
            public string Asset { get; set; }
            public string AskOrder { get; set; }
            public string BidOrder { get; set; }
            public int AskOrderHeight { get; set; }
            public string Seller { get; set; }
            public string SellerRS { get; set; }
            public string Buyer { get; set; }
            public string BuyerRS { get; set; }
            public string Block { get; set; }
            public int Height { get; set; }
            public string TradeType { get; set; }
            public string Name { get; set; }
            public int Decimals { get; set; }
            public string Price { get; set; }
        
    }
}
