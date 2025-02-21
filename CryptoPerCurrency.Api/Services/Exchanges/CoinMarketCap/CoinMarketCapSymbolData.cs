namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

public class CoinMarketCapSymbolData
{
    public int Id { get; set; }
    public int Rank { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int IsActive { get; set; }
    public int Status { get; set; }
    public DateTime FirstHistoricalData { get; set; }
    public DateTime LastHistoricalData { get; set; }
}