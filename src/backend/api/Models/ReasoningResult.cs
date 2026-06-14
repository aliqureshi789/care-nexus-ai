namespace CareNexus.Api.Models;

public class ReasoningResult
{
    public string Recommendation { get; set; } = string.Empty;
    public string Confidence { get; set; } = string.Empty;
    public List<string> Reasoning { get; set; } = new();
    public string NextAction { get; set; } = string.Empty;

    //Foundry IQ
    public List<string> Explanation { get; set; } = new();
}
