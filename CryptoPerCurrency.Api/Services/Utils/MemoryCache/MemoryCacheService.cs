using Microsoft.Extensions.Caching.Memory;

namespace CryptoPerCurrency.Api.Services.Utils.MemoryCache;


public class MemoryCacheService<T> : IMemoryCacheService<T>
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get(string key)
    {
        _cache.TryGetValue(key, out T value);
        return value;
    }

    public void Set(string key, T value, TimeSpan duration)
    {
        _cache.Set(key, value, duration);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}