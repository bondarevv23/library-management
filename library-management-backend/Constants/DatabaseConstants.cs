namespace LibraryManagementSystem.Constants;

public static class DatabaseConstants
{
    public const string TABLE_NAME_AUTHORS = "authors";

    public const string TABLE_NAME_BOOKS = "books";

    public const string DEFAULT_CONNECTION_NAME = "DefaultConnection";

    public const string DEFAULT_REDIS_CONNECTION = "Redis:ConnectionString";

    public const int DEFAULT_LEVENSHTAIN_DISTANCE = 5;

    public const bool PERFORM_FUZZY_SEARCH_IN_MEMORY = false;

    public const int FUZZY_SEARCH_BUCKET_SIZE = 1000;
}
