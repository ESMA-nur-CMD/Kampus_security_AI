namespace KampusGuvenlikAI.Core.Models;

public class AppSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://api.openai.com/v1/chat/completions";
    public string Model { get; set; } = "gpt-4o-mini";
    public string DatabasePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KampusGuvenlikAI",
        "campus_security.db");
    public bool UseRuleBasedFallback { get; set; } = true;
}
