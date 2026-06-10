namespace KampusGuvenlikAI.Core.Models;

public class AnalysisResult
{
    public string Category { get; set; } = string.Empty;
    public string Urgency { get; set; } = string.Empty;
    public string Step1 { get; set; } = string.Empty;
    public string Step2 { get; set; } = string.Empty;
    public string Step3 { get; set; } = string.Empty;
    public string RawResponse { get; set; } = string.Empty;
}
