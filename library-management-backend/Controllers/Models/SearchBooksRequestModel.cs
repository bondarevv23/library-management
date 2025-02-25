using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class SearchBooksRequestModel : IPagable, IHasOperationDetails
{
    public string? UrlEncodedSearchQuery { get; set; }

    public string? PageNumberPathVariable { get; set; }

    public string? PageSizePathVariable { get; set; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string OperationDetails =>
        $"Fuzzy search of '{TABLE_NAME_BOOKS}' records by title with query = '{UrlEncodedSearchQuery}'";
}
