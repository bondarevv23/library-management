namespace LibraryManagementSystem.Controllers.Models;

public interface IHasIdPathVariable
{
    string? IdPathVariable { get; set; }

    long? Id { get; set; }
}
