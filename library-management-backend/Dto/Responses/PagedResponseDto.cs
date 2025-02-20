using Newtonsoft.Json;

namespace LibraryManagementSystem.Dto.Responses;

[method: JsonConstructor]
public record PagedResponseDto<T>(
    int PageNumber,
    int PageSize,
    bool HasNextPage,
    bool HasPreviousPage,
    int TotalRecords,
    int TotalPages,
    IList<T> Data
)
{
    public PagedResponseDto(IList<T> data, int totalRecords, int pageSize, int pageNumber)
        : this(
            pageNumber,
            pageSize,
            pageNumber * pageSize < totalRecords,
            pageNumber > 1,
            totalRecords,
            (totalRecords + pageSize - 1) / pageSize,
            data
        )
    {
    }
}