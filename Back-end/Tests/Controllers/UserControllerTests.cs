using BLL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task GetAllUsersAsync_ReturnsOkResultWithUsers()
    {
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Mock<IJwtValidationService> jwtService = new Mock<IJwtValidationService>();
        userServiceMock.Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(GetSampleUserResponseDtoList());

        UserController controller = new UserController(userServiceMock.Object, jwtService.Object);
        IActionResult result = await controller.GetAllUsersAsync();

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        IEnumerable<UserResponseDto> users = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);

        Assert.NotNull(users);
        userServiceMock.Verify(s => s.GetAllUsersAsync(), Times.Once);
    }

    private IEnumerable<UserResponseDto> GetSampleUserResponseDtoList()
    {
        return new List<UserResponseDto>
        {
            new UserResponseDto { Email = "@1", Name = "User1" },
            new UserResponseDto { Email = "@2", Name = "User2" },
        };
    }
    
    [Fact]
    public async Task LoginAsync_ReturnsOkResultWithToken()
    {
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.CheckCredentialsAsync(It.IsAny<LoginRequestDto>()));
        userServiceMock.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { UserId = 1, Email = "user@example.com" });

        Mock<IJwtValidationService> jwtValidationServiceMock = new Mock<IJwtValidationService>();
        jwtValidationServiceMock.Setup(s => s.GenerateToken(It.IsAny<User>()))
            .Returns("sampleToken"); 

        UserController controller = new UserController(userServiceMock.Object, jwtValidationServiceMock.Object);

        IActionResult result = await controller.LoginAsync(new LoginRequestDto { Email = "user@example.com", Password = "password" });

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        string token = Assert.IsType<string>(okResult.Value);

        Assert.NotNull(token);
        Assert.Equal("sampleToken", token); 
        userServiceMock.Verify(s => s.CheckCredentialsAsync(It.IsAny<LoginRequestDto>()), Times.Once);
        userServiceMock.Verify(s => s.GetUserByEmailAsync("user@example.com"), Times.Once);
        jwtValidationServiceMock.Verify(s => s.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsOkResultWithMessage()
    {
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Mock<IJwtValidationService> jwtService = new Mock<IJwtValidationService>();
        userServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequestDto>()));

        UserController controller = new UserController(userServiceMock.Object, jwtService.Object);

        IActionResult result = await controller.CreateUserAsync(new CreateUserRequestDto
        {
            Name = "TestUser",
            Email = "testuser@example.com"
        });

        Assert.IsType<OkObjectResult>(result);
        userServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<CreateUserRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsOkResultWithMessage()
    {
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Mock<IJwtValidationService> jwtService = new Mock<IJwtValidationService>();
        userServiceMock.Setup(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequestDto>()));

        UserController controller = new UserController(userServiceMock.Object, jwtService.Object);

        IActionResult result = await controller.UpdateUserAsync(new UpdateUserRequestDto
        {
            UserId = 1,
            Name = "UpdatedUser"
        });

        Assert.IsType<OkObjectResult>(result);
        userServiceMock.Verify(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ReturnsOkResultWithMessage()
    {
        Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Mock<IJwtValidationService> jwtService = new Mock<IJwtValidationService>();
        userServiceMock.Setup(s => s.DeleteUserAsync(It.IsAny<string>()));

        UserController controller = new UserController(userServiceMock.Object, jwtService.Object);

        IActionResult result = await controller.DeleteUserAsync("test@example.com");

        Assert.IsType<OkObjectResult>(result);
        userServiceMock.Verify(s => s.DeleteUserAsync(It.IsAny<string>()), Times.Once);
    }


}