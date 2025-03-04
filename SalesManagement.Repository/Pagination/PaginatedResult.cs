namespace SalesManagement.Repository.Pagination;

public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public int TotalPages => (int)Math.Ceiling(count / (double)pageSize);

    public IEnumerable<TEntity> Data { get; } = data;
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex + 1 < TotalPages;
}