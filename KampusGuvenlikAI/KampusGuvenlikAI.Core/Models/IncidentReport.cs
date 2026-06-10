namespace KampusGuvenlikAI.Core.Models;

public class IncidentReport
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string ReportText { get; set; } = string.Empty;
    public string HumanLabel { get; set; } = string.Empty;
    public string AiCategory { get; set; } = string.Empty;
    public string Urgency { get; set; } = string.Empty;
    public string Step1 { get; set; } = string.Empty;
    public string Step2 { get; set; } = string.Empty;
    public string Step3 { get; set; } = string.Empty;

    public string ComparisonStatus =>
        string.Equals(HumanLabel, AiCategory, StringComparison.OrdinalIgnoreCase)
            ? "Uyumlu"
            : "Farkli";
}
