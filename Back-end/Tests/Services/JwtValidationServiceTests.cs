using BLL.Services;
using BLL.Services.Authentication;
using DAL.Models;
using Microsoft.Extensions.Configuration;

namespace Tests.Services;

public class JwtValidationServiceTests
{
    [Fact]
    public void GenerateToken_ShouldGenerateToken()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", "iRAW38828BzlnM3tJFcPiuCmZdUcM9ng" },
                { "Jwt:Issuer", "http://localhost:5050" },
                { "Jwt:Audience", "http://localhost:5050" }
            })
            .Build();

        User user = new User { UserId = 1, Name = "test", Email = "test@example.com" };
        JwtValidationService tokenService = new JwtValidationService(configuration);

        string generatedToken = tokenService.GenerateToken(user);

        Assert.NotNull(generatedToken);
        Assert.NotEmpty(generatedToken);
    }

    [Fact]
    public void GenerateToken_ShouldThrowException_WhenConfigurationIsMissing()
    {
        // Create a configuration with missing Jwt:Key
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Issuer", "your_issuer" },
                { "Jwt:Audience", "your_audience" }
            })
            .Build();

        User user = new User { Email = "test@example.com" };
        JwtValidationService tokenService = new JwtValidationService(configuration);

        Assert.Throws<InvalidOperationException>(() => tokenService.GenerateToken(user));
    }

    [Fact]
    public void GenerateToken_ShouldThrowException_WhenUserIsNull()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", "your_secret_key_with_sufficient_length" },
                { "Jwt:Issuer", "your_issuer" },
                { "Jwt:Audience", "your_audience" }
            })
            .Build();

        User user = new User(); 
        JwtValidationService tokenService = new JwtValidationService(configuration);

        Assert.Throws<ArgumentNullException>(() => tokenService.GenerateToken(user));
    }


}