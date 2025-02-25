using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Controllers.Models;

public class CreateBookRequestModel : IHasOperationDetails
{
    public BookRequestDto? Body { get; set; }

    public string OperationDetails => $"Create '{TABLE_NAME_BOOKS}' record";
}
