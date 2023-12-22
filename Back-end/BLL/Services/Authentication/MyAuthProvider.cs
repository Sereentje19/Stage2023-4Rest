using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BLL.Services.Authentication
{
    public class MyAuthProvider : IAuthenticationProvider
    {
        private string _accessToken;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _authority;
        private readonly string[] _scopes;

        public MyAuthProvider(string clientId, string clientSecret, string authority, string[] scopes)
        {
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._authority = authority;
            this._scopes = scopes;
        }

        public async Task AuthenticateRequestAsync(RequestInformation requestInfo,
            Dictionary<string, object> additionalRequestFeatures, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await GetAccessToken();
            }

            if (requestInfo.Headers is RequestHeaders headers && headers != null)
            {
                Console.WriteLine($"Setting Access Token: {_accessToken}");
                headers.Add("Authorization", "Bearer " + _accessToken);
            }
        }

        private async Task<string> GetAccessToken()
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(new Uri(_authority))
                .Build();

            AuthenticationResult authenticationResult = await confidentialClientApplication
                .AcquireTokenForClient(_scopes)
                .ExecuteAsync();

            return authenticationResult.AccessToken;
        }
    }
}