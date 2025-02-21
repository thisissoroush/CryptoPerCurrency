
namespace CryptoPerCurrency.Api.Primitives.Models;

public class CryptoRateModel
{
    public CryptoRateModel(string coin)
    {
        Coin = coin;
        Rates = new();
    }
    public string Coin { get; private set; }
    public Dictionary<string,decimal> Rates { get; private set; }
    
    
    public void SetRate(Dictionary<string, decimal> rates)
    {
        Rates = rates;
    }
    
}