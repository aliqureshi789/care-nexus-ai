using CareNexus.Api.DTOs;
using CareNexus.Api.Models;

namespace CareNexus.Api.Services.Interfaces;

public interface IKnowledgeService
{
    Task<KnowledgeResult> RetrieveAsync(
        AgentRequestDto request,
        WorkContextResult context);
}
