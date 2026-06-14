namespace CareNexus.Api.DTOs;

public class AgentResponseDto
{
    public string RequestId { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public string Confidence { get; set; } = string.Empty;

    public List<string> Reasoning { get; set; } = new();
    public List<string> Explanation { get; set; } = new();

    public string PolicyReference { get; set; } = string.Empty;
    public string PolicySummary { get; set; } = string.Empty;
    public string NextAction { get; set; } = string.Empty;

    public string Citation { get; set; } = string.Empty;
    public string SourceSystem { get; set; } = string.Empty;
    public List<string> Evidence { get; set; } = new();

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}