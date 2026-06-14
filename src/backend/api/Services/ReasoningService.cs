using CareNexus.Api.DTOs;
using CareNexus.Api.Models;
using CareNexus.Api.Services.Interfaces;

namespace CareNexus.Api.Services;

public class ReasoningService : IReasoningService
{
    public Task<ReasoningResult> EvaluateAsync(
        AgentRequestDto request,
        WorkContextResult context,
        KnowledgeResult knowledge)
    {
        var query = request.Query?.ToLowerInvariant() ?? string.Empty;

        var result = new ReasoningResult
        {
            Recommendation = "Approve",
            Confidence = "Medium",
            NextAction = "Send for final approval",
            Reasoning = new List<string>
            {
                "Request context was successfully identified.",
                "A relevant policy was matched.",
                "No escalation keyword was detected in the request.",
                "The request can proceed under the current workflow policy."
            },
            //Foundry IQ
            Explanation = new List<string>
            {
                $"Matched policy: {knowledge.Citation}",
                $"Source system: {knowledge.SourceSystem}",
                $"Evidence used: {knowledge.PolicySummary}",
                "Recommendation derived from enterprise policy rules."
            }

        };

        if (request.Query.Contains("missing", StringComparison.OrdinalIgnoreCase) ||
            request.Query.Contains("incomplete", StringComparison.OrdinalIgnoreCase))
        {
            result.Recommendation = "Escalate";
            result.Confidence = "High";
            result.NextAction = "Request missing information";
            result.Reasoning = new List<string>
            {
                "The request indicates incomplete or missing information.",
                "Policy requires complete supporting information before approval.",
                "The next best action is to request clarification or missing documents."
            };
            result.Explanation = new List<string>
            {
                $"Matched policy: {knowledge.Citation}",
            $"Source system: {knowledge.SourceSystem}",
            $"Evidence used: {knowledge.PolicySummary}",
            "The request contains missing or incomplete information, so escalation is required."
            };

        }

        //Foundry IQ
        if(query.Contains("security") || query.Contains("protected systems") || query.Contains("credentials"))
        {
                result.Recommendation = "Escalate";
                result.Confidence = "High";
                result.NextAction = "Route to security review";
                result.Reasoning = new List<string>
            {
                $"The request was evaluated against policy {knowledge.PolicyReference}.",
                $"The matched policy was '{knowledge.PolicyTitle}'.",
                "The request appears to affect security-sensitive controls or protected systems.",
                "Security-sensitive requests should not proceed directly to approval."
            };
                result.Explanation = new List<string>
            {
                $"Matched policy: {knowledge.Citation}",
                $"Source system: {knowledge.SourceSystem}",
                $"Evidence used: {knowledge.PolicySummary}",
                "The request affects protected systems or controls, so it should be routed to security review."
            };
        }
        //if (request.Query.Contains("reject", StringComparison.OrdinalIgnoreCase))
        //{
        //    result.Recommendation = "Reject";
        //    result.Confidence = "Medium";
        //    result.NextAction = "Close request";
        //    result.Reasoning = new List<string>
        //    {
        //        "The request content includes explicit rejection criteria.",
        //        "Approval conditions are not satisfied based on the provided input."
        //    };
        //}

        return Task.FromResult(result);
    }
}

