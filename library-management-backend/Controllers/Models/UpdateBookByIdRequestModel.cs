using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class UpdateBookByIdRequestModel : IHasIdPathVariable, IHasOperationDetails
{
    public string? IdPathVariable { get; set; }

    public long? Id { get; set; }
    
    public UpdateBookRequestDto? Body { get; set; }

    public string OperationDetails => $"Update '{TABLE_NAME_BOOKS}' record by id = '{IdPathVariable}'";
}
