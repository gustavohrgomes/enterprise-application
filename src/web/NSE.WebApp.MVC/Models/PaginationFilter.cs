namespace NSE.WebApp.MVC.Models;

public record PaginationFilter(int PageSize = 8, int PageIndex = 1, string Query = null);
