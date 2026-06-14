using Microsoft.Identity.Client;

namespace CareNexus.Api.Services;

public class GraphAuthService
{
	private readonly IConfiguration _config;

	public GraphAuthService(IConfiguration config)
	{
		_config = config;
	}

	public async Task<string> GetAccessTokenAsync()
	{
		var tenantId = _config["AzureAd:TenantId"];
		var clientId = _config["AzureAd:ClientId"];
		var clientSecret = _config["AzureAd:ClientSecret"];

		var app = ConfidentialClientApplicationBuilder
			.Create(clientId)
			.WithClientSecret(clientSecret)
			.WithAuthority($"https://login.microsoftonline.com/{tenantId}")
			.Build();

		var result = await app
			.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
			.ExecuteAsync();

		return result.AccessToken;
	}
}