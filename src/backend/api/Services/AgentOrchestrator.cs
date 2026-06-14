using CareNexus.Api.DTOs;
using CareNexus.Api.Services.Interfaces;

namespace CareNexus.Api.Services;

public class AgentOrchestrator : IAgentOrchestrator
{
    private readonly IWorkContextService _workContextService;
    private readonly IKnowledgeService _knowledgeService;
    private readonly IReasoningService _reasoningService;

    public AgentOrchestrator(
        IWorkContextService workContextService,
        IKnowledgeService knowledgeService,
        IReasoningService reasoningService)
    {
        _workContextService = workContextService;
        _knowledgeService = knowledgeService;
        _reasoningService = reasoningService;
    }

    public async Task<AgentResponseDto> ProcessAsync(AgentRequestDto request)
    {
        var context = await _workContextService.AnalyseAsync(request);
        var knowledge = await _knowledgeService.RetrieveAsync(request, context);
        var reasoning = await _reasoningService.EvaluateAsync(request, context, knowledge);

        return new AgentResponseDto
        {
            RequestId = request.RequestId,
            Recommendation = reasoning.Recommendation,
            Confidence = reasoning.Confidence,
            Reasoning = reasoning.Reasoning,
            PolicyReference = knowledge.PolicyReference,
            PolicySummary = knowledge.PolicySummary,
            NextAction = reasoning.NextAction,
            //SourceFile = knowledge.SourceFile,

            //Foundry IQ
            Citation = knowledge.Citation,
            SourceSystem = knowledge.SourceSystem,
            Evidence = knowledge.Evidence,


            TimestampUtc = DateTime.UtcNow
        };
    }

    public Task<List<string>> ExplainAsync(string query, string recommendation)
    {
        var explanation = new List<string>
        {
            $"The recommendation '{recommendation}' was generated based on request content analysis.",
            "The system considered workflow context, policy match, and request wording.",
            "This MVP uses deterministic logic and policy mapping for explainability."
        };

        return Task.FromResult(explanation);
    }
}