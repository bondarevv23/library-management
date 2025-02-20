namespace LibraryManagementSystem.Controllers.Models;

public interface IPagable
{
    public string? PageNumberPathVariable { get; set; }

    public string? PageSizePathVariable { get; set; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }
}
