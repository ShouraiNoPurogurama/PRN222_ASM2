﻿namespace SalesManagement.Repository.Pagination;

public record PaginationRequest(int PageIndex = 0, int PageSize = 5);