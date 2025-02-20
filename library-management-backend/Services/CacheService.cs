using StackExchange.Redis;
using LibraryManagementSystem.Services;
using Newtonsoft.Json;

public class CacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task Save<T>(string key, string hashKey, T value)
    {
        var serializedValue = JsonConvert.SerializeObject(value);
        await _database.HashSetAsync(key, hashKey, serializedValue);
    }

    public async Task<T?> Get<T>(string key, string hashKey)
    {
        var redisValue = await _database.HashGetAsync(key, hashKey);
        
        return redisValue.IsNullOrEmpty
            ? default
            : JsonConvert.DeserializeObject<T>(redisValue!);
    }

    public async Task DeleteByKey(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}