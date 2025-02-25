using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class CreateAuthorRequestModel : IHasOperationDetails
{
    public AuthorRequestDto? Body { get; set; }

    public string OperationDetails => $"Create '{TABLE_NAME_AUTHORS}' record";
}
