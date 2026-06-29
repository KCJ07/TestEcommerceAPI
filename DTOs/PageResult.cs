// DTO class for pagnation

public class PageResult<T>
{
    public int TotalCount{get; set;}

    public int Page{get; set;}

    public int PageSize{get; set;}

    public int TotalPages{get; set;}

    public required List<T> Data{get; set;}
}