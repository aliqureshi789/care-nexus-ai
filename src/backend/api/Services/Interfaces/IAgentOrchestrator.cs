using CareNexus.Api.DTOs;

namespace CareNexus.Api.Services.Interfaces;

public interface IAgentOrchestrator
{
    Task<AgentResponseDto> ProcessAsync(AgentRequestDto request);
    Task<List<string>> ExplainAsync(string query, string recommendation);
}