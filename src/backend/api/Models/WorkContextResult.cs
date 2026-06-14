namespace CareNexus.Api.Models;

public class WorkContextResult
{
    public string UserRole { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string WorkflowType { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
}