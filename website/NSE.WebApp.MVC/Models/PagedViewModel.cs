namespace NSE.WebApp.MVC.Models;

public class PagedViewModel<T> : IPagedResult where T : class
{
    public string ReferenceAction { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public string Query { get; set; }
    public IEnumerable<T> Records { get; set; }
}

public interface IPagedResult
{
    public string ReferenceAction { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public string Query { get; set; }
}
