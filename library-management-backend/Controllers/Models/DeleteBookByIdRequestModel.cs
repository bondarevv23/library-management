using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class DeleteBookByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { get; set; }
    
    public long? Id { get; set; }

    public string OperationDetails => $"Delete '{TABLE_NAME_BOOKS}' record by id = '{IdPathVariable}'";
}
