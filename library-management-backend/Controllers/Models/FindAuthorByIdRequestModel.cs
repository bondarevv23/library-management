using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class FindAuthorByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { set; get; }

    public long? Id { set; get; }

    public string OperationDetails => $"Find '{TABLE_NAME_AUTHORS}' record by id = '{IdPathVariable}'";
}
