﻿namespace EventManagement.Domain.Common;

public record Paging(int PageNumber = 1, int PageSize = 10)
{
    public int PageNumber { get; init; } = Math.Max(1, PageNumber);
    public int PageSize { get; init; } = Math.Clamp(PageSize, 1, 100);
}
