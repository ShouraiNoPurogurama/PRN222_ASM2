namespace SalesManagement.Repositories.Pagination;

public record PaginationRequest(int PageIndex = 0, int PageSize = 5);