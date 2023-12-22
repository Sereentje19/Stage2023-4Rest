﻿using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Repositories;
using Moq;

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
}