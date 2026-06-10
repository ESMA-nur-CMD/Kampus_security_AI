namespace KampusGuvenlikAI.Core.Services;

public static class TextSanitizer
{
    public static string CleanIncidentText(string input)
    {
        var cleaned = (input ?? string.Empty)
            .Trim()
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\t", " ");

        while (cleaned.Contains("  ", StringComparison.Ordinal))
        {
            cleaned = cleaned.Replace("  ", " ");
        }

        return cleaned.ToUpperInvariant();
    }
}
