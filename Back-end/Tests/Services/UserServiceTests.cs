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
        Mock<IUserService> mockUserService = new Mock<IUserService>();
        Mock<IJwtValidationService> mockJwtValidationService = new Mock<IJwtValidationService>();

        IEnumerable<UserResponseDto> fakeUsers = new List<UserResponseDto>
        {
            new UserResponseDto { Email = "@1", Name = "User1" },
            new UserResponseDto { Email = "@2", Name = "User2" }
        };
        
        mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(fakeUsers);
        UserController userController = new UserController(mockUserService.Object, mockJwtValidationService.Object);

        IActionResult result = await userController.GetAllUsersAsync();
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
        const string userEmail = "test@example.com";
        User expectedUser = new User
        {
            UserId = 1,
            Email = userEmail,
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userEmail)).ReturnsAsync(expectedUser);

        IUserService userService = new UserService(mockUserRepository.Object); 

        User resultUser = await userService.GetUserByEmailAsync(userEmail);

        Assert.NotNull(resultUser);
        Assert.Equal(expectedUser.UserId, resultUser.UserId);
        Assert.Equal(expectedUser.Email, resultUser.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        const string userEmail = "nonexistent@example.com";

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(userEmail)).ReturnsAsync((User)null);

        IUserService userService = new UserService(mockUserRepository.Object);
        User resultUser = await userService.GetUserByEmailAsync(userEmail);

        Assert.Null(resultUser);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCallRepository_WhenUserRequestIsProvided()
    {
        CreateUserRequestDto userRequest = new CreateUserRequestDto
        {
            Email = "test@example.com",
            Name = "John Doe",
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); 

        await userService.CreateUserAsync(userRequest);
        
        User userRequest2 = new User
        {
            Email = "test@example.com",
            Name = "John Doe",
        };

        mockUserRepository.Verify(repo => repo.CreateUserAsync(userRequest2), Times.Once);
    }
    
     [Fact]
    public async Task GetAllUsersAsync_ShouldReturnListOfUserResponseDto_WhenUsersExist()
    {
        IEnumerable<UserResponseDto> mockUsers = new List<UserResponseDto>
        {
            new UserResponseDto {  Name = "User1", Email = "user1@example.com" },
            new UserResponseDto { Name = "User2", Email = "user2@example.com" },
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(mockUsers);

        IUserService userService = new UserService(mockUserRepository.Object); 
        IEnumerable<UserResponseDto> result = await userService.GetAllUsersAsync();

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
        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(Enumerable.Empty<UserResponseDto>());

        IUserService userService = new UserService(mockUserRepository.Object); 
        IEnumerable<UserResponseDto> result = await userService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallRepository_WhenEmailIsProvided()
    {
        const string userEmail = "test@example.com";

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); 

        await userService.DeleteUserAsync(userEmail);
        mockUserRepository.Verify(repo => repo.DeleteUserAsync(userEmail), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallUpdateUserName_WhenUpdateNameIsTrue()
    {
        UpdateUserRequestDto updateUserRequestDto = new UpdateUserRequestDto
        {
            UserId = 1,
            Email1 = "oldemail@example.com",
            Name = "NewName",
            UpdateName = true,
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); 

        await userService.UpdateUserAsync(updateUserRequestDto);

        mockUserRepository.Verify(repo => repo.UpdateUserNameAsync(It.IsAny<User>()), Times.Once);
        mockUserRepository.Verify(repo => repo.UpdateUserEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallUpdateUserEmail_WhenUpdateNameIsFalse()
    {
        UpdateUserRequestDto updateUserRequestDto = new UpdateUserRequestDto
        {
            UserId = 1,
            Email1 = "oldemail@example.com",
            Email2 = "newemail@example.com",
            Name = "NewName",
            UpdateName = false,
        };

        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        IUserService userService = new UserService(mockUserRepository.Object); 

        await userService.UpdateUserAsync(updateUserRequestDto);

        mockUserRepository.Verify(repo => repo.UpdateUserEmailAsync(It.IsAny<User>(), updateUserRequestDto.Email2), Times.Once);
        mockUserRepository.Verify(repo => repo.UpdateUserNameAsync(It.IsAny<User>()), Times.Never);
    }
    
    
}