using CareNexus.Api.DTOs;
using CareNexus.Api.Models;

namespace CareNexus.Api.Services.Interfaces;

public interface IReasoningService
{
    Task<ReasoningResult> EvaluateAsync(AgentRequestDto request, WorkContextResult context, KnowledgeResult knowledge);
}