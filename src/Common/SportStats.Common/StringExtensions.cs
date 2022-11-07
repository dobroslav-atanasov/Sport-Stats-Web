namespace SportStats.Common;

using System.Net;

public static class StringExtensions
{
    public static string Decode(this string text)
    {
        return WebUtility.HtmlDecode(text);
    }
}