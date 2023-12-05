using BLL.Services;
using DAL.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using PL.Exceptions;
using PL.Models;
using PL.Models.Requests;

namespace Tests.Services;

public class PasswordresetServiceTests
{
    [Fact]
    public async Task PostResetCode_ShouldGenerateCodeAndSendEmail()
    {
        string email = "test@example.com";
        string code = "123456"; 

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        mockPasswordResetRepository
            .Setup(repo => repo.PostResetCode(It.IsAny<string>(), email))
            .ReturnsAsync(new User { Name = "Test User" }); 

        Mock<IMailService> mockMailService = new Mock<IMailService>();

        Mock<IOptions<MailSettings>> mockMailSettingsOptions = new Mock<IOptions<MailSettings>>();
        mockMailSettingsOptions.Setup(x => x.Value).Returns(new MailSettings());

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockMailService.Object
        );

        await passwordResetService.PostResetCode(email);

        mockPasswordResetRepository.Verify(repo => repo.PostResetCode(It.IsAny<string>(), email), Times.Once);
        mockMailService.Verify(
            mailService => mailService.SendPasswordEmail(code, email, "Verificatie code.", "Test User"),
            Times.Once
        );
    }
    
    [Fact]
    public async Task CheckEnteredCode_ShouldCallPasswordResetRepository()
    {
        string email = "test@example.com";
        string code = "123456"; 

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            Mock.Of<IMailService>() 
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
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object
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

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IMailService> mockExceptionService = new Mock<IMailService>();
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object
        );

        await passwordResetService.PostPassword(request);

        mockPasswordResetRepository.Verify(
            repo => repo.PostPassword(request.Email, request.Password1, request.Code),
            Times.Once
        );
    }
    

}