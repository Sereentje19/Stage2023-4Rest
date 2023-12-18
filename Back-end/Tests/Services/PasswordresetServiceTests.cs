using BLL.Interfaces;
using BLL.Services;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Repositories;
using DAL.Settings;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Services;

public class PasswordresetServiceTests
{
    
    [Fact]
    public async Task PostResetCode_ShouldGenerateCodeAndSendEmail()
    {
        const string email = "test@example.com";

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();

        mockPasswordResetRepository
            .Setup(repo => repo.PostResetCode(It.IsAny<string>(), email))
            .ReturnsAsync(new User { Name = "Test User", Email = "test@example.com" });

        Mock<IMailService> mockMailService = new Mock<IMailService>();
        Mock<ILoginService> mockLoginService = new Mock<ILoginService>();

        Mock<IOptions<MailSettings>> mockMailSettingsOptions = new Mock<IOptions<MailSettings>>();
        mockMailSettingsOptions.Setup(x => x.Value).Returns(new MailSettings());

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockMailService.Object,
            mockLoginService.Object
        );

        mockPasswordResetRepository
            .Setup(repo => repo.PostResetCode(It.IsAny<string>(), email))
            .Callback<string, string>((c, _) => { })
            .ReturnsAsync(new User { Name = "Test User", Email = "test@example.com" });

        await passwordResetService.PostResetCode(email);

        mockMailService.Verify(
            mailService => mailService.SendPasswordEmail(
                It.IsAny<string>(), 
                email,
                "Verificatie code.",
                "Test User"
            ),
            Times.Once
        );
    }

    
    [Fact]
    public async Task CheckEnteredCode_ShouldCallPasswordResetRepository()
    {
        string email = "test@example.com";
        string code = "123456"; 

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<ILoginService> mockLoginService = new Mock<ILoginService>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            Mock.Of<IMailService>() ,
            mockLoginService.Object
        );

        await passwordResetService.CheckEnteredCode(email, code);

        mockPasswordResetRepository.Verify(
            repo => repo.CheckEnteredCode(email, code),
            Times.Once
        );
    }

    [Fact]
    public async Task PostPassword_ShouldThrowException_WhenPasswordsAreNotEqual()
    {
        PasswordChangeRequest request = new PasswordChangeRequest
        {
            Email = "test@example.com",
            Password1 = "password1",
            Password2 = "differentpassword",
            Code = "123456" 
        };

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IMailService> mockExceptionService = new Mock<IMailService>();
        Mock<ILoginService> mockLoginService = new Mock<ILoginService>();
        
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object,
            mockLoginService.Object
        );

        await Assert.ThrowsAsync<InputValidationException>(() => passwordResetService.PostPassword(request));

        mockPasswordResetRepository.Verify(
            repo => repo.PostPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never
        );
    }

    [Fact]
    public async Task PostPassword_ShouldCallRepository_WhenPasswordsAreEqual()
    {
        PasswordChangeRequest request = new PasswordChangeRequest
        {
            Email = "test@example.com",
            Password1 = "password",
            Password2 = "password",
            Code = "123456" 
        };

        Mock<ILoginService> mockLoginService = new Mock<ILoginService>();
        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IMailService> mockExceptionService = new Mock<IMailService>();
        
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object,
            mockLoginService.Object
        );

        await passwordResetService.PostPassword(request);

        mockPasswordResetRepository.Verify(
            repo => repo.PostPassword(request.Email, request.Password1, request.Code),
            Times.Once
        );
    }
    

}