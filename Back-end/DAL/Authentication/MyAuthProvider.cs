using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace DAL.Authentication
{
    public class MyAuthProvider : IAuthenticationProvider
{
    private string accessToken;
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string authority;
    private readonly string[] scopes;

    public MyAuthProvider(string clientId, string clientSecret, string authority, string[] scopes)
    {
        this.clientId = clientId;
        this.clientSecret = clientSecret;
        this.authority = authority;
        this.scopes = scopes;
    }

    public async Task AuthenticateRequestAsync(RequestInformation requestInfo, Dictionary<string, object>? additionalRequestFeatures, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(accessToken) || IsAccessTokenExpired())
            {
                accessToken = await GetAccessToken();
            }

            if (requestInfo.Headers is IDictionary<string, object> headers)
            {
                // Set the access token in the Authorization header
                Console.WriteLine($"Received Access Token: {accessToken}");
                headers["Authorization"] = $"Bearer {accessToken}";
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately (e.g., log, rethrow, etc.)
            Console.WriteLine($"Error in AuthenticateRequestAsync: {ex.Message}");
            throw;
        }
    }

    private async Task<string> GetAccessToken()
    {
        try
        {
            Console.WriteLine($"Acquiring token for Client ID: {clientId}");

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var authenticationResult = await confidentialClientApplication
                .AcquireTokenForClient(scopes)
                .ExecuteAsync();

           

            return authenticationResult.AccessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAccessToken: {ex.Message}");
            throw;
        }
    }


    private bool IsAccessTokenExpired()
    {
        // Implement your logic to check if the access token is expired
        // You may need to track the token expiration time and compare it with the current time
        return false; // Replace with your logic
    }
}

}