namespace LibraryManagementSystem.Services;

public interface ICacheService
{
    Task Save<T>(string key, string hashKey, T value);

    Task<T?> Get<T>(string key, string hashKey);

    Task DeleteByKey(string key);
}
