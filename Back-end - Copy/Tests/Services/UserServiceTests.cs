using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task CheckCredentials_ShouldReturnUser_WhenCredentialsAreValid()
    {
        LoginRequestDto userCredentials = new LoginRequestDto
        {
            Email = "validUsername",
            Password = "validPassword"
        };

        User expectedUser = new User
        {
            UserId = 1,
        };

        Mock<IUserRepository> loginRepositoryMock = new Mock<IUserRepository>();
        loginRepositoryMock.Setup(repo => repo.CheckCredentialsAsync(userCredentials))
            .ReturnsAsync(expectedUser);

        UserService userService = new UserService(loginRepositoryMock.Object);
        User resultUser = await userService.CheckCredentialsAsync(userCredentials);

        Assert.NotNull(resultUser);
        Assert.Equal(expectedUser.UserId, resultUser.UserId);
    }

    [Fact]
    public async Task CheckCredentials_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        LoginRequestDto userCredentials = new LoginRequestDto
        {
            Email = "invalidUsername",
            Password = "invalidPassword"
        };

        Mock<IUserRepository> loginRepositoryMock = new Mock<IUserRepository>();
        loginRepositoryMock.Setup(repo => repo.CheckCredentialsAsync(userCredentials))
            .ReturnsAsync((User)null);

        UserService userService = new UserService(loginRepositoryMock.Object);
        User resultUser = await userService.CheckCredentialsAsync(userCredentials);

        Assert.Null(resultUser);
    }
    
    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnOkResultWithUsers()
    {
        // Arrange
        Mock<IUserService> mockUserService = new Mock<IUserService>();
        Mock<IJwtValidationService> mockJwtValidationService = new Mock<IJwtValidationService>();

        IEnumerable<UserResponseDto> fakeUsers = new List<UserResponseDto>
        {
            new UserResponseDto { Email = "@1", Name = "User1" },
            new UserResponseDto { Email = "@2", Name = "User2" }
            // Add more fake users as needed for your test cases
        };
        mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(fakeUsers);

        UserController userController = new UserController(mockUserService.Object, mockJwtValidationService.Object);

        // Act
        IActionResult result = await userController.GetAllUsersAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);

        OkObjectResult okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        IEnumerable<UserResponseDto> returnedUsers = okResult.Value as IEnumerable<UserResponseDto>;
        Assert.NotNull(returnedUsers);

        Assert.Equal(fakeUsers, returnedUsers);
    }
    
    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        string userEmail = "test@example.com";
        User expectedUser = new User
        {
            UserId = 1,
            Email = userEmail,
            // Set other properties as needed for your test case
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userEmail)).ReturnsAsync(expectedUser);

        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        User resultUser = await userService.GetUserByEmailAsync(userEmail);

        // Assert
        Assert.NotNull(resultUser);
        Assert.Equal(expectedUser.UserId, resultUser.UserId);
        Assert.Equal(expectedUser.Email, resultUser.Email);
        // Add assertions for other properties as needed
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        string userEmail = "nonexistent@example.com";

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userEmail)).ReturnsAsync((User)null);

        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        User resultUser = await userService.GetUserByEmailAsync(userEmail);

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCallRepository_WhenUserRequestIsProvided()
    {
        // Arrange
        CreateUserRequestDto userRequest = new CreateUserRequestDto
        {
            Email = "test@example.com",
            Name = "John Doe",
            // Set other properties as needed for your test case
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        await userService.CreateUserAsync(userRequest);

        // Assert
        mockUserRepository.Verify(repo => repo.CreateUserAsync(userRequest), Times.Once);
    }
    
     [Fact]
    public async Task GetAllUsersAsync_ShouldReturnListOfUserResponseDto_WhenUsersExist()
    {
        // Arrange
        IEnumerable<UserResponseDto> mockUsers = new List<UserResponseDto>
        {
            new UserResponseDto {  Name = "User1", Email = "user1@example.com" },
            new UserResponseDto { Name = "User2", Email = "user2@example.com" },
            // Add more mock users as needed for your test case
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(mockUsers);

        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        IEnumerable<UserResponseDto> result = await userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(result);

        List<UserResponseDto> resultList = result.ToList();
        Assert.Equal(mockUsers.Count(), resultList.Count);

        for (int i = 0; i < mockUsers.Count(); i++)
        {
            Assert.Equal(mockUsers.ElementAt(i).Name, resultList.ElementAt(i).Name);
            Assert.Equal(mockUsers.ElementAt(i).Email, resultList.ElementAt(i).Email);
        }
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(Enumerable.Empty<UserResponseDto>());

        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        IEnumerable<UserResponseDto> result = await userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallRepository_WhenEmailIsProvided()
    {
        // Arrange
        string userEmail = "test@example.com";

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        await userService.DeleteUserAsync(userEmail);

        // Assert
        mockUserRepository.Verify(repo => repo.DeleteUserAsync(userEmail), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallUpdateUserName_WhenUpdateNameIsTrue()
    {
        // Arrange
        UpdateUserRequestDto updateUserRequestDto = new UpdateUserRequestDto
        {
            UserId = 1,
            Email1 = "oldemail@example.com",
            Name = "NewName",
            UpdateName = true,
            // Set other properties as needed for your test case
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        await userService.UpdateUserAsync(updateUserRequestDto);

        // Assert
        mockUserRepository.Verify(repo => repo.UpdateUserNameAsync(It.IsAny<User>()), Times.Once);
        mockUserRepository.Verify(repo => repo.UpdateUserEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallUpdateUserEmail_WhenUpdateNameIsFalse()
    {
        // Arrange
        UpdateUserRequestDto updateUserRequestDto = new UpdateUserRequestDto
        {
            UserId = 1,
            Email1 = "oldemail@example.com",
            Email2 = "newemail@example.com",
            Name = "NewName",
            UpdateName = false,
            // Set other properties as needed for your test case
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); // Assuming you have a UserService class implementing IUserService

        // Act
        await userService.UpdateUserAsync(updateUserRequestDto);

        // Assert
        mockUserRepository.Verify(repo => repo.UpdateUserEmailAsync(It.IsAny<User>(), updateUserRequestDto.Email2), Times.Once);
        mockUserRepository.Verify(repo => repo.UpdateUserNameAsync(It.IsAny<User>()), Times.Never);
    }
    
    
}