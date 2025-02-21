namespace LibraryManagementSystem.Data;

public class ApplicationSettings
{
    public int BookSearchMaxLevenshteinDistance { get; set; }
    public bool PerformFuzzySearchInMemory { get; set; }
    public int FuzzySearchBucketSize { get; set; }
}