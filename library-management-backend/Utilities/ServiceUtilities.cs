using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Exceptions;
using static LibraryManagementSystem.Constants.PaginationConstants;

namespace LibraryManagementSystem.Utilities;

public static class ServiceUtilities
{
    public static void EnrichWithId(IHasIdPathVariable request)
    {
        var id = long.Parse(request.IdPathVariable!);
        request.Id = id;
    }

    public static void EnrichWithPagableData(IPagable pagable)
    {
        pagable.PageNumber = int.TryParse(pagable.PageNumberPathVariable, out int pageNumber)
            ? pageNumber
            : DEFAULT_PAGE_NUMBER;
        pagable.PageSize = int.TryParse(pagable.PageSizePathVariable, out int pageSize)
            ? pageSize
            : DEFAULT_PAGE_SIZE;
    }

    public static void EnrichErrorLogDescriptionWithOperationDetails(
        LibraryManagementSystemException exception,
        IHasOperationDetails request
    )
    {
        exception.LogDescription =
            $"{request.OperationDetails}: failed with error \"{exception.LogDescription}\"";
    }
}
