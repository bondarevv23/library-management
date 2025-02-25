namespace LibraryManagementSystem.Constants;

public static class RedisConstants
{
    public const string AUTHORS_REDIS_KEY = "authors";
    public const string AUTHORS_HASH_KEY_TEMPLATE = "authors:pageNumber:{0}:pageSize:{1}";
}
