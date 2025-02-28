using CryptoPerCurrency.Api.Features.Rates;
using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;
using CryptoPerCurrency.Api.Services.Utils.MemoryCache;
using Quartz;

namespace CryptoPerCurrency.Api.Services.Utils.Quartz;

[DisallowConcurrentExecution]
public class CryptoRatesJobs : IJob
{
    private readonly IMemoryCacheService<string[]> _memoryCache;
    private readonly CoinMarketCapClient _client;
    private readonly RateProccesor _rateProccesor;
    private readonly CancellationToken _cancelationToken = new CancellationTokenSource().Token;
    private readonly string _symbolsKey = "Symbols";
    
    //check for new coins
    private readonly int _symbolsCacheDurationInMinute = 180; 
    
    //top 100 coin
    private readonly int _symbolsMaxRank = 100; 
    
    //if we send all the request in one time, Ratelimiter will activate and reject all the request so we should wait
    private readonly int _rateLimiterDelay = 3000; 
    
    public CryptoRatesJobs(
        CoinMarketCapClient client, 
        RateProccesor rateProccesor, 
        IMemoryCacheService<string[]> memoryCache)
    {
        _client = client;
        _rateProccesor = rateProccesor;
        _memoryCache = memoryCache;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var result = _memoryCache.Get(_symbolsKey);

        if (result is null)
        { 
            var response = await _client.GetSymbolsAsync(_cancelationToken);
            
            result = response
                .Data
                .Where(r => r.Rank <= _symbolsMaxRank)
                .Select(r => r.Symbol)
                .Distinct()
                .ToArray();
            
            _memoryCache.Set(_symbolsKey,result, TimeSpan.FromMinutes(_symbolsCacheDurationInMinute));    
        }
           
        List<Task> tasks = new List<Task>();
        foreach (var symbol in result)
        {
            tasks.Add(Task.Run(() => InvokeProcessor(symbol)));
        }
        
        await Task.WhenAll(tasks);
    }

    private async Task InvokeProcessor(string symbol)
    {
        //At first it was without delay since i realiside the rate limiter and ip blocker of CoinMarketCap
        await Task.Delay(_rateLimiterDelay);
        await _rateProccesor.ProcessAsync(_cancelationToken, new RateRequest(symbol));
    }
    

}