using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class DeleteAuthorByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { get; set; }
    
    public long? Id { get; set; }

    public string OperationDetails => $"Delete '{TABLE_NAME_AUTHORS}' record by id = '{IdPathVariable}'";
}
