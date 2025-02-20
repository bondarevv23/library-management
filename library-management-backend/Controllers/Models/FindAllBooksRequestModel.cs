using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class FindAllBooksRequestModel : IPagable, IHasOperationDetails
{
    public string? PageNumberPathVariable { get; set; }

    public string? PageSizePathVariable { get; set; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string OperationDetails =>
        $"Find all '{TABLE_NAME_BOOKS}' records with pageNumber = '{PageNumberPathVariable}' and pageSize = '{PageSizePathVariable}";
}
