using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class FindAllBooksByAuthorIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { get; set; }
    
    public long? Id { get; set; }

    public string OperationDetails => $"Find all '{TABLE_NAME_BOOKS}' records by authorId = '{IdPathVariable}'";
}