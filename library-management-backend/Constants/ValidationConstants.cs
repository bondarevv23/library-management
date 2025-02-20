namespace LibraryManagementSystem.Constants;

public static class ValidationConstants
{
    public const string INSTANCE_NAME_REQUEST_DATA = "Request data";

    public const string BODY_NAME_FILED = "body.name";
    public const string BODY_DATE_OF_BIRTH_FIELD = "body.dateOfBirth";
    public const string BODY_TITLE_FILED = "body.title";
    public const string BODY_PUBLICATION_YEAR_FIELD = "body.publicationYear";
    public const string BODY_AUTHOR_ID_FIELD = "body.authorId";
    public const string BODY_FIELD = "body";
    public const string URI_VARIABLE_QUERY_FIELD = "query";
    
    public const string ID_PATH_VARIABLE_FIELD = "id";
    public const string PAGE_NUMBER_PATH_VARIABLE_FIELD = "pageNumber";
    public const string PAGE_SIZE_PATH_VARIABLE_FIELD = "pageSize";

    public const int AUTHOR_NAME_MAX_LENGTH = 255;
    public const int AUTHOR_NAME_MIN_LENGTH = 3;
    public const int BOOK_TITLE_MAX_LENGTH = 255;
    public const int BOOK_TITLE_MIN_LENGTH = 1;
    public const int PAGE_SIZE_MAX_VALUE = 300;
    public const int QUERY_MIN_LENGTH = 1;
    public const int QUERY_MAX_LENGTH = 600;

    public const string NOT_NULL_CONSTRAINT = "must not be null";
    public const string MIN_MAX_LENGTH_CONSTRAINT = "must be between {0} and {1} characters long";
    public const string POSITIVE_CONSTRAINT = "must be positive";
    public const string POSITIVE_VALID_LONG_CONSTRAINT = "must be a positive valid long";
    public const string POSITIVE_VALID_INT_CONSTRAINT = "must be a valid positive int";
    public const string POSITIVE_VALID_INT_MULTIPLE_10 = "must be a valid positive int, multiple of 10";
}
