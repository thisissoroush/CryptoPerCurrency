namespace CryptoPerCurrency.Api.Services.Utils.MemoryCache;

public interface IMemoryCacheService<T>
{
    T? Get(string key);
    void Set(string key, T value, TimeSpan duration);
    void Remove(string key);
}