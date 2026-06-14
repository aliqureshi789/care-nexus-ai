using System.Net.Http.Headers;
using System.Text.Json;

namespace CareNexus.Api.Services;

public class SharePointService
{
    private readonly GraphAuthService _authService;
    private readonly IConfiguration _config;
    private readonly ILogger<SharePointService> _logger;
    private readonly HttpClient _httpClient;

    public SharePointService(
        GraphAuthService authService,
        IConfiguration config,
        ILogger<SharePointService> logger)
    {
        _authService = authService;
        _config = config;
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<List<SharePointDocument>> GetPolicyDocumentsAsync()
    {
        var token = await _authService.GetAccessTokenAsync();
        var siteId = _config["SharePoint:SiteId"] ?? string.Empty;
        var libraryName = _config["SharePoint:LibraryName"] ?? "Documents";

        if (string.IsNullOrWhiteSpace(siteId))
        {
            throw new Exception("SharePoint:SiteId is missing in appsettings.json");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // STEP A: Get all drives (document libraries) for the site
        var drivesUrl = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drives";
        var drivesResponse = await _httpClient.GetAsync(drivesUrl);

        if (!drivesResponse.IsSuccessStatusCode)
        {
            var errorBody = await drivesResponse.Content.ReadAsStringAsync();
            _logger.LogError("Graph drives request failed. Status: {StatusCode}. Body: {Body}",
                drivesResponse.StatusCode, errorBody);

            throw new Exception($"Failed to retrieve drives from SharePoint. Status: {drivesResponse.StatusCode}. Body: {errorBody}");
        }

        var drivesJson = await drivesResponse.Content.ReadAsStringAsync();
        using var drivesDoc = JsonDocument.Parse(drivesJson);

        string? driveId = null;

        foreach (var drive in drivesDoc.RootElement.GetProperty("value").EnumerateArray())
        {
            var name = drive.GetProperty("name").GetString() ?? string.Empty;
            if (name.Equals(libraryName, StringComparison.OrdinalIgnoreCase))
            {
                driveId = drive.GetProperty("id").GetString();
                break;
            }
        }

        if (string.IsNullOrWhiteSpace(driveId))
        {
            throw new Exception($"Could not find SharePoint document library '{libraryName}' for the configured site.");
        }

        // STEP B: List root children of that library
        var childrenUrl = $"https://graph.microsoft.com/v1.0/drives/{driveId}/root/children";
        var childrenResponse = await _httpClient.GetAsync(childrenUrl);

        if (!childrenResponse.IsSuccessStatusCode)
        {
            var errorBody = await childrenResponse.Content.ReadAsStringAsync();
            _logger.LogError("Graph children request failed. Status: {StatusCode}. Body: {Body}",
                childrenResponse.StatusCode, errorBody);

            throw new Exception($"Failed to retrieve files from library '{libraryName}'. Status: {childrenResponse.StatusCode}. Body: {errorBody}");
        }

        var childrenJson = await childrenResponse.Content.ReadAsStringAsync();
        using var childrenDoc = JsonDocument.Parse(childrenJson);

        var results = new List<SharePointDocument>();

        foreach (var item in childrenDoc.RootElement.GetProperty("value").EnumerateArray())
        {
            // Keep only files, not folders
            if (!item.TryGetProperty("file", out _))
                continue;

            var fileId = item.GetProperty("id").GetString() ?? string.Empty;
            var fileName = item.GetProperty("name").GetString() ?? string.Empty;

            // For hackathon reliability, only process text / markdown files directly
            if (!(fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ||
                  fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("Skipping non-text file: {FileName}", fileName);
                continue;
            }

            var content = await DownloadFileContentAsync(driveId, fileId);

            results.Add(new SharePointDocument
            {
                Id = fileId,
                Name = fileName,
                Content = content
            });
        }

        return results;
    }

    private async Task<string> DownloadFileContentAsync(string driveId, string itemId)
    {
        var token = await _authService.GetAccessTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var contentUrl = $"https://graph.microsoft.com/v1.0/drives/{driveId}/items/{itemId}/content";
        var response = await _httpClient.GetAsync(contentUrl);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            _logger.LogError("Graph content request failed. Status: {StatusCode}. Body: {Body}",
                response.StatusCode, errorBody);

            throw new Exception($"Failed to download SharePoint file content. Status: {response.StatusCode}. Body: {errorBody}");
        }

        return await response.Content.ReadAsStringAsync();
    }
}

public class SharePointDocument
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
