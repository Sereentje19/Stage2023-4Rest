using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BLL.Services.Authentication
{
    public class AuthProvider : IAuthenticationProvider
    {
        private string _accessToken;
        private const string ClientId = "133fe8d9-7037-4c05-bd81-7724b52de083";
        private const string ClientSecret = "APM8Q~YatdGa2S~TETexhKQeflJ8f3Fy2LicIdqO";
        private const string Authority = "https://login.microsoftonline.com/347afa52-35d4-48fc-abb4-b04cb1ff683e";
        private readonly string[] _scopes = { "https://graph.microsoft.com/.default" };

        public async Task AuthenticateRequestAsync(RequestInformation  requestInfo, Dictionary<string, object>? additionalAuthenticationContext = default, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await GetAccessToken();
            }

            if (requestInfo.Headers is { } headers)
            {
                headers.Add("Authorization", "Bearer " + _accessToken);
            }
        }

        private async Task<string> GetAccessToken()
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri(Authority))
                .Build();

            AuthenticationResult authenticationResult = await confidentialClientApplication
                .AcquireTokenForClient(_scopes)
                .ExecuteAsync();

            return authenticationResult.AccessToken;
        }
    }
}