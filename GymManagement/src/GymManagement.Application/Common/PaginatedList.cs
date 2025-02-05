namespace GymManagement.Application.Common;

public class PaginatedList<T>(List<T> items, int pageNumber, int pageSize, int totalCount)
{
    public List<T> Items { get; init; } = items;

    public int PageNumber { get; init; } = pageNumber;

    public int PageSize { get; } = pageSize;

    public int TotalCount { get; } = totalCount;

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}