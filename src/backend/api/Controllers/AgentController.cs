using CareNexus.Api.DTOs;
using CareNexus.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CareNexus.Api.Controllers;

[ApiController]
[Route("agent")]
public class AgentController : ControllerBase
{
    private readonly IAgentOrchestrator _agentOrchestrator;

    public AgentController(IAgentOrchestrator agentOrchestrator)
    {
        _agentOrchestrator = agentOrchestrator;
    }

    [HttpPost("process")]
    public async Task<ActionResult<AgentResponseDto>> Process([FromBody] AgentRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("Query is required.");
        }

        var response = await _agentOrchestrator.ProcessAsync(request);
        return Ok(response);
    }

    [HttpPost("explain")]
    public async Task<ActionResult<List<string>>> Explain([FromBody] ExplainRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("Query is required.");
        }

        var response = await _agentOrchestrator.ExplainAsync(request.Query, request.Recommendation);
        return Ok(response);
    }
}
