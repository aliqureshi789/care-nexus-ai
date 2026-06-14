namespace CareNexus.Api.Models;

public class KnowledgeResult
{
    public string PolicyTitle { get; set; } = string.Empty;
    public string PolicyReference { get; set; } = string.Empty;
    public string PolicySummary { get; set; } = string.Empty;
    public string RelevantText { get; set; } = string.Empty;
    //public string SourceFile { get; set; } = string.Empty;



    // Foundry IQ fields

    public string SourceSystem { get; set; } = string.Empty;
    public string Citation { get; set; } = string.Empty;
    public List<string> Evidence { get; set; } = new();

}