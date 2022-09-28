namespace NSE.Catalogo.API.Models;

public record PagedResultFilter(int PageSize = 8, int PageIndex = 1, string Query = null);
