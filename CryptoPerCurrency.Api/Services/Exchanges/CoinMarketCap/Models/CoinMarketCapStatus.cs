namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

public class CoinMarketCapStatus
{
    public string Timestamp { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public int Elapsed { get; set; }
    public int CreditCount { get; set; }
    public string Notice { get; set; }
}