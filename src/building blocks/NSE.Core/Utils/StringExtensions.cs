namespace NSE.Core.Utils;

public static class StringExtensions
{
    public static string ApenasNumeros(this string input) => new(input.Where(char.IsDigit).ToArray());
}
