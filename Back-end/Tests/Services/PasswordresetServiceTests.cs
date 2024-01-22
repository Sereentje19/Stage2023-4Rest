using BLL.Interfaces;
using BLL.Services;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
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
            .Setup(repo => repo.CreateResetCodeAsync(It.IsAny<string>(), email))
            .ReturnsAsync(new User { Name = "Test User", Email = "test@example.com" });

        Mock<IMailService> mockMailService = new Mock<IMailService>();
        Mock<IUserService> mockLoginService = new Mock<IUserService>();

        Mock<IOptions<MailSettings>> mockMailSettingsOptions = new Mock<IOptions<MailSettings>>();
        mockMailSettingsOptions.Setup(x => x.Value).Returns(new MailSettings());

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockMailService.Object,
            mockLoginService.Object
        );

        mockPasswordResetRepository
            .Setup(repo => repo.CreateResetCodeAsync(It.IsAny<string>(), email))
            .Callback<string, string>((c, _) => { })
            .ReturnsAsync(new User { Name = "Test User", Email = "test@example.com" });

        await passwordResetService.CreateResetCodeAsync(email);

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
        const string email = "test@example.com";
        const string code = "123456"; 

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IUserService> mockLoginService = new Mock<IUserService>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            Mock.Of<IMailService>() ,
            mockLoginService.Object
        );

        await passwordResetService.CheckEnteredCodeAsync(email, code);

        mockPasswordResetRepository.Verify(
            repo => repo.CheckEnteredCodeAsync(email, code),
            Times.Once
        );
    }

    [Fact]
    public async Task PostPassword_ShouldThrowException_WhenPasswordsAreNotEqual()
    {
        CreatePasswordRequestDto requestDto = new CreatePasswordRequestDto
        {
            Email = "test@example.com",
            Password1 = "password1",
            Password2 = "differentpassword",
            Code = "123456" 
        };

        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IMailService> mockExceptionService = new Mock<IMailService>();
        Mock<IUserService> mockLoginService = new Mock<IUserService>();
        
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object,
            mockLoginService.Object
        );

        await Assert.ThrowsAsync<InputValidationException>(() => passwordResetService.CreatePasswordAsync(requestDto));

        mockPasswordResetRepository.Verify(
            repo => repo.CreatePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never
        );
    }

    [Fact]
    public async Task PostPassword_ShouldCallRepository_WhenPasswordsAreEqual()
    {
        CreatePasswordRequestDto requestDto = new CreatePasswordRequestDto
        {
            Email = "test@example.com",
            Password1 = "password",
            Password2 = "password",
            Code = "123456" 
        };

        Mock<IUserService> mockLoginService = new Mock<IUserService>();
        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        Mock<IMailService> mockExceptionService = new Mock<IMailService>();
        
        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            mockExceptionService.Object,
            mockLoginService.Object
        );

        await passwordResetService.CreatePasswordAsync(requestDto);

        mockPasswordResetRepository.Verify(
            repo => repo.CreatePasswordAsync(requestDto.Email, requestDto.Password1, requestDto.Code),
            Times.Once
        );
    }
    
    [Fact]
    public async Task UpdatePassword_ShouldCallCheckCredentialsAsync_Once()
    {
        UpdatePasswordRequestDto requestDto = new UpdatePasswordRequestDto
        {
            Email = "test@example.com",
            Password1 = "oldPassword",
            Password2 = "newPassword",
            Password3 = "newPassword",
            Name = "TestUser",
            PasswordHash = "oldHash",
            PasswordSalt = "oldSalt",
            UserId = 1
        };

        Mock<IUserService> mockUserService = new Mock<IUserService>();
        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            null, 
            mockUserService.Object
        );

        await passwordResetService.UpdatePasswordAsync(requestDto);

        mockUserService.Verify(
            userService => userService.CheckCredentialsAsync(It.IsAny<LoginRequestDto>()),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdatePassword_ShouldThrowException_WhenPasswordsNotEqual()
    {
        UpdatePasswordRequestDto requestDto = new UpdatePasswordRequestDto
        {
            Email = "test@example.com",
            Password1 = "oldPassword",
            Password2 = "newPassword",
            Password3 = "differentPassword",
            Name = "TestUser",
            PasswordHash = "oldHash",
            PasswordSalt = "oldSalt",
            UserId = 1
        };

        Mock<IUserService> mockUserService = new Mock<IUserService>();
        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            null, 
            mockUserService.Object
        );

        await Assert.ThrowsAsync<InputValidationException>(() => passwordResetService.UpdatePasswordAsync(requestDto));
    }

    [Fact]
    public async Task UpdatePassword_ShouldCallUpdatePasswordAsync_Once()
    {
        UpdatePasswordRequestDto requestDto = new UpdatePasswordRequestDto
        {
            Email = "test@example.com",
            Password1 = "oldPassword",
            Password2 = "newPassword",
            Password3 = "newPassword",
            Name = "TestUser",
            PasswordHash = "oldHash",
            PasswordSalt = "oldSalt",
            UserId = 1
        };

        Mock<IUserService> mockUserService = new Mock<IUserService>();
        Mock<IPasswordResetRepository> mockPasswordResetRepository = new Mock<IPasswordResetRepository>();

        PasswordResetService passwordResetService = new PasswordResetService(
            mockPasswordResetRepository.Object,
            null, 
            mockUserService.Object
        );

        await passwordResetService.UpdatePasswordAsync(requestDto);

        mockPasswordResetRepository.Verify(
            repo => repo.UpdatePasswordAsync(It.IsAny<User>(), requestDto.Password2),
            Times.Once
        );
    }
}