using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class FindBookByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { set; get; }

    public long? Id { set; get; }

    public string OperationDetails =>
        $"Find '{TABLE_NAME_BOOKS}' record by id = '{IdPathVariable}'";
}
