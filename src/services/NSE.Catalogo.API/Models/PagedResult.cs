namespace NSE.Catalogo.API.Models;

public class PagedResult<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => Convert.ToInt32(Math.Ceiling((double)TotalRecords / PageSize));
    public int TotalRecords { get; set; } 
    public string Query { get; set; }
    public IEnumerable<T> Records { get; set; }
}
