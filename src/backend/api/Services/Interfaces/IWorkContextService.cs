using CareNexus.Api.DTOs;
using CareNexus.Api.Models;

namespace CareNexus.Api.Services.Interfaces;

public interface IWorkContextService
{
    Task<WorkContextResult> AnalyseAsync(AgentRequestDto request);
}