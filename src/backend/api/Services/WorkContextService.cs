using CareNexus.Api.DTOs;
using CareNexus.Api.Models;
using CareNexus.Api.Services.Interfaces;

namespace CareNexus.Api.Services;

public class WorkContextService : IWorkContextService
{
    public Task<WorkContextResult> AnalyseAsync(AgentRequestDto request)
    {
        var workflowType = string.IsNullOrWhiteSpace(request.RequestType)
            ? "Approval Review"
            : request.RequestType;

        var result = new WorkContextResult
        {
            UserRole = string.IsNullOrWhiteSpace(request.UserRole) ? "Staff" : request.UserRole,
            Department = string.IsNullOrWhiteSpace(request.Department) ? "Operations" : request.Department,
            WorkflowType = workflowType,
            Priority = request.Query.Contains("urgent", StringComparison.OrdinalIgnoreCase)
                ? "High"
                : "Normal"
        };

        return Task.FromResult(result);
    }
}
