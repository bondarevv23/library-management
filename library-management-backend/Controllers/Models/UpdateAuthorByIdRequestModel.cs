using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class UpdateAuthorByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { get; set; }

    public long? Id { get; set; }
    
    public AuthorRequestDto? Body { get; set; }

    public string OperationDetails => $"Update '{TABLE_NAME_AUTHORS}' record by id = '{IdPathVariable}'";
}
