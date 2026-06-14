using CareNexus.Api.DTOs;
using CareNexus.Api.Models;
using CareNexus.Api.Services.Interfaces;

namespace CareNexus.Api.Services;

public class KnowledgeService : IKnowledgeService
{
    private readonly SharePointService _sharePointService;

    public KnowledgeService(SharePointService sharePointService)
    {
        _sharePointService = sharePointService;
    }

    public async Task<KnowledgeResult> RetrieveAsync(
        AgentRequestDto request,
        WorkContextResult context)
    {
        var documents = await _sharePointService.GetPolicyDocumentsAsync();

        if (documents == null || documents.Count == 0)
        {
            return new KnowledgeResult
            {
                PolicyTitle = "Default Policy",
                PolicyReference = "POL-DEFAULT",
                PolicySummary = "No policy documents found in SharePoint.",
                RelevantText = "No policy content available.",
                SourceSystem = "SharePoint",
                Citation = "No citation available",
                Evidence = new List<string>()
            };
        }

        var selected = documents.First();

        return new KnowledgeResult
        {
            PolicyTitle = selected.Name,
            PolicyReference = "SP-DOC",
            PolicySummary = selected.Content.Length > 200
                ? selected.Content.Substring(0, 200)
                : selected.Content,
            RelevantText = selected.Content,
            SourceSystem = "SharePoint",
            Citation = $"SharePoint Document: {selected.Name}",
            Evidence = new List<string>
            {
                selected.Content.Substring(0, Math.Min(150, selected.Content.Length))
            }
        };
    }
}



#region
//using CareNexus.Api.DTOs;
//using CareNexus.Api.Models;
//using CareNexus.Api.Services.Interfaces;

//namespace CareNexus.Api.Services;

//public class KnowledgeService : IKnowledgeService
//{
//    //Foundry IQ
//    private readonly SharePointService _spService;

//    private readonly ILogger<KnowledgeService> _logger;

//    public KnowledgeService(ILogger<KnowledgeService> logger)
//    {
//        _logger = logger;
//    }


//    //Foundry IQ
//    public KnowledgeService(SharePointService spService)
//    {
//        _spService = spService;
//    }

//    //Foundry IQ


//    public async Task<KnowledgeResult> RetrieveAsync(AgentRequestDto request, WorkContextResult context)public async Task<KnowledgeResult> var documents = await _sharePointService.GetPolicyDocumentsAsync();

//    if (documents == null || documents.Count == 0)
//    {
//        return new KnowledgeResult
//        {
//            PolicyTitle = "Default Workflow Policy",
//            PolicyReference = "POL-DEFAULT-000",
//            PolicySummary = "No SharePoint policy documents were found.",
//            RelevantText = "The SharePoint document library returned no policy files.",
//            SourceSystem = "SharePoint",
//            Citation = "No citation available",
//            Evidence = new List<string>()
//};
//    }

//    var queryText = $"{request.Query} {request.RequestType} {context.WorkflowType} {context.Department}".ToLowerInvariant();

//var bestMatch = documents
//    .Select(doc => new
//    {
//        Document = doc,
//        Score = CalculateScore(doc.Content.ToLowerInvariant(), queryText)
//    })
//    .OrderByDescending(x => x.Score)
//    .First()
//    .Document;

//return new KnowledgeResult
//{
//    PolicyTitle = bestMatch.Name,
//    PolicyReference = "SP-DOC",
//    PolicySummary = bestMatch.Content.Length > 200 ? bestMatch.Content.Substring(0, 200) : bestMatch.Content,
//    RelevantText = bestMatch.Content,
//    SourceSystem = "SharePoint",
//    Citation = $"SharePoint Document: {bestMatch.Name}",
//    Evidence = new List<string>
//        {
//            bestMatch.Content.Length > 150 ? bestMatch.Content.Substring(0, 150) : bestMatch.Content
//        }
//};
//}


//    #region
//    //public Task<KnowledgeResult> RetrieveAsync(AgentRequestDto request, WorkContextResult context)
//    //{
//    //    var policiesPath = ResolvePoliciesPath();

//    //    if (!Directory.Exists(policiesPath))
//    //    {
//    //        _logger.LogWarning("Policies folder not found at: {PoliciesPath}", policiesPath);

//    //        return Task.FromResult(new KnowledgeResult
//    //        {
//    //            PolicyTitle = "Default Workflow Policy",
//    //            PolicyReference = "POL-DEFAULT-000",
//    //            PolicySummary = "No policy files were found. Default policy response was returned.",
//    //            RelevantText = "The system could not locate policy files under data/sample/policies.",
//    //            SourceFile = string.Empty
//    //        });
//    //    }

//    //    var policyFiles = Directory.GetFiles(policiesPath, "*.md", SearchOption.TopDirectoryOnly);

//    //    if (policyFiles.Length == 0)
//    //    {
//    //        _logger.LogWarning("No markdown policy files found in: {PoliciesPath}", policiesPath);

//    //        return Task.FromResult(new KnowledgeResult
//    //        {
//    //            PolicyTitle = "Default Workflow Policy",
//    //            PolicyReference = "POL-DEFAULT-001",
//    //            PolicySummary = "No markdown policy files were found. Default policy response was returned.",
//    //            RelevantText = "The policy folder exists, but no .md files were found.",
//    //            SourceFile = string.Empty
//    //        });
//    //    }

//    //    var policyDocuments = policyFiles
//    //        .Select(ParsePolicyFile)
//    //        .Where(p => p != null)
//    //        .Cast<PolicyDocument>()
//    //        .ToList();

//    //    if (!policyDocuments.Any())
//    //    {
//    //        return Task.FromResult(new KnowledgeResult
//    //        {
//    //            PolicyTitle = "Default Workflow Policy",
//    //            PolicyReference = "POL-DEFAULT-002",
//    //            PolicySummary = "Policy files were found but could not be parsed.",
//    //            RelevantText = "No valid policy metadata could be extracted from markdown files.",
//    //            SourceFile = string.Empty
//    //        });
//    //    }

//    //    var bestMatch = GetBestMatchingPolicy(policyDocuments, request, context);

//    //    return Task.FromResult(new KnowledgeResult
//    //    {
//    //        PolicyTitle = bestMatch.Title,
//    //        PolicyReference = bestMatch.Reference,
//    //        PolicySummary = bestMatch.Summary,
//    //        RelevantText = bestMatch.Body,
//    //        SourceFile = bestMatch.FileName
//    //    });
//    //}
//    #endregion

//    private string ResolvePoliciesPath()
//{
//    // Start from the running directory and walk upwards until the policy folder is found.
//    var current = new DirectoryInfo(AppContext.BaseDirectory);

//    while (current != null)
//    {
//        var candidate = Path.Combine(current.FullName, "data", "sample", "policies");
//        if (Directory.Exists(candidate))
//        {
//            return candidate;
//        }

//        current = current.Parent;
//    }

//    // Fallback to local relative path
//    return Path.Combine(Directory.GetCurrentDirectory(), "data", "sample", "policies");
//}

//private PolicyDocument? ParsePolicyFile(string filePath)
//{
//    try
//    {
//        var lines = File.ReadAllLines(filePath);

//        string title = string.Empty;
//        string reference = string.Empty;
//        string summary = string.Empty;

//        var bodyLines = new List<string>();

//        foreach (var rawLine in lines)
//        {
//            var line = rawLine.Trim();

//            if (string.IsNullOrWhiteSpace(line))
//            {
//                continue;
//            }

//            if (line.StartsWith("# "))
//            {
//                title = line.Replace("# ", "").Trim();
//                continue;
//            }

//            if (line.StartsWith("Reference:", StringComparison.OrdinalIgnoreCase))
//            {
//                reference = line.Substring("Reference:".Length).Trim();
//                continue;
//            }

//            if (line.StartsWith("Summary:", StringComparison.OrdinalIgnoreCase))
//            {
//                summary = line.Substring("Summary:".Length).Trim();
//                continue;
//            }

//            bodyLines.Add(line);
//        }

//        if (string.IsNullOrWhiteSpace(title))
//        {
//            title = Path.GetFileNameWithoutExtension(filePath);
//        }

//        if (string.IsNullOrWhiteSpace(reference))
//        {
//            reference = "POL-UNKNOWN";
//        }

//        if (string.IsNullOrWhiteSpace(summary))
//        {
//            summary = bodyLines.FirstOrDefault() ?? "No summary available.";
//        }

//        return new PolicyDocument
//        {
//            Title = title,
//            Reference = reference,
//            Summary = summary,
//            Body = string.Join(" ", bodyLines),
//            FileName = Path.GetFileName(filePath)
//        };
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "Failed to parse policy file: {FilePath}", filePath);
//        return null;
//    }
//}

//private PolicyDocument GetBestMatchingPolicy(
//    List<PolicyDocument> policies,
//    AgentRequestDto request,
//    WorkContextResult context)
//{
//    var queryText = $"{request.Query} {request.RequestType} {context.WorkflowType} {context.Department}".ToLowerInvariant();

//    var scored = policies
//        .Select(policy => new
//        {
//            Policy = policy,
//            Score = CalculateScore(policy, queryText)
//        })
//        .OrderByDescending(x => x.Score)
//        .ToList();

//    var best = scored.FirstOrDefault();

//    if (best == null || best.Score <= 0)
//    {
//        return policies.First();
//    }

//    return best.Policy;
//}

//private int CalculateScore(PolicyDocument policy, string queryText)
//{
//    int score = 0;

//    var searchText = $"{policy.Title} {policy.Reference} {policy.Summary} {policy.Body}".ToLowerInvariant();

//    var keywords = queryText
//        .Split(new[] { ' ', ',', '.', ';', ':', '-' }, StringSplitOptions.RemoveEmptyEntries)
//        .Where(w => w.Length > 2)
//        .Distinct();

//    foreach (var keyword in keywords)
//    {
//        if (searchText.Contains(keyword))
//        {
//            score++;
//        }
//    }

//    // Stronger weighting for common workflow signals
//    if (queryText.Contains("security") && searchText.Contains("security"))
//        score += 5;

//    if ((queryText.Contains("missing") || queryText.Contains("incomplete")) &&
//        (searchText.Contains("missing") || searchText.Contains("incomplete") || searchText.Contains("escalat")))
//        score += 5;

//    if (queryText.Contains("approve") && searchText.Contains("approval"))
//        score += 5;

//    return score;
//}

//private class PolicyDocument
//{
//    public string Title { get; set; } = string.Empty;
//    public string Reference { get; set; } = string.Empty;
//    public string Summary { get; set; } = string.Empty;
//    public string Body { get; set; } = string.Empty;
//    public string FileName { get; set; } = string.Empty;
//}
//}
#endregion
