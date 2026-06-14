namespace CareNexus.Api.DTOs;

public class AgentRequestDto
{
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;

    //FoundryIQ


    public string Citation { get; set; } = string.Empty;
    public string SourceSystem { get; set; } = string.Empty;
    public List<string> Evidence { get; set; } = new();

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;



}