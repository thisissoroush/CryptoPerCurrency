namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

public class CoinMarketCapQuote
{
    public decimal Price { get; set; }
    public decimal Volume24h { get; set; }
    public decimal VolumeChange24h { get; set; }
    public decimal PercentChange1h { get; set; }
    public decimal PercentChange24h { get; set; }
    public decimal PercentChange7d { get; set; }
    public decimal PercentChange30d { get; set; }
    public decimal PercentChange60d { get; set; }
    public decimal PercentChange90d { get; set; }
    public decimal MarketCap { get; set; }
    public decimal MarketCapDominance { get; set; }
    public decimal FullyDilutedMarketCap { get; set; }
    public object Tvl { get; set; }
    public DateTime LastUpdated { get; set; }
}