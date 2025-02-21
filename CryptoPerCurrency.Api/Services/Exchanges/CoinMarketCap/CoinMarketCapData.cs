namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;


public class CoinMarketCapData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int NumMarketPairs { get; set; }
    public DateTime DateAdded { get; set; }
    public List<string> Tags { get; set; }
    public long MaxSupply { get; set; }
    public long CirculatingSupply { get; set; }
    public long TotalSupply { get; set; }
    public int IsActive { get; set; }
    public bool InfiniteSupply { get; set; }
    public object Platform { get; set; } // This could be a specific class if needed
    public int CmcRank { get; set; }
    public int IsFiat { get; set; }
    public object SelfReportedCirculatingSupply { get; set; }
    public object SelfReportedMarketCap { get; set; }
    public object TvlRatio { get; set; }
    public DateTime LastUpdated { get; set; }
    public Dictionary<string, CoinMarketCapQuote> Quote { get; set; }
}