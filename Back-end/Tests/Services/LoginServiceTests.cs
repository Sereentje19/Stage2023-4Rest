﻿using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Repositories;
using Moq;

namespace Tests.Services;

public class LoginServiceTests
{
    [Fact]
    public async Task CheckCredentials_ShouldReturnUser_WhenCredentialsAreValid()
    {
        LoginRequestDTO userCredentials = new LoginRequestDTO
        {
            Email = "validUsername",
            Password = "validPassword"
        };

        User expectedUser = new User
        {
            UserId = 1,
        };

        Mock<ILoginRepository> loginRepositoryMock = new Mock<ILoginRepository>();
        loginRepositoryMock.Setup(repo => repo.CheckCredentials(userCredentials))
            .ReturnsAsync(expectedUser);

        UserService userService = new UserService(loginRepositoryMock.Object);
        User resultUser = await userService.CheckCredentials(userCredentials);

        Assert.NotNull(resultUser);
        Assert.Equal(expectedUser.UserId, resultUser.UserId);
    }

    [Fact]
    public async Task CheckCredentials_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        LoginRequestDTO userCredentials = new LoginRequestDTO
        {
            Email = "invalidUsername",
            Password = "invalidPassword"
        };

        Mock<ILoginRepository> loginRepositoryMock = new Mock<ILoginRepository>();
        loginRepositoryMock.Setup(repo => repo.CheckCredentials(userCredentials))
            .ReturnsAsync((User)null);

        UserService userService = new UserService(loginRepositoryMock.Object);
        User resultUser = await userService.CheckCredentials(userCredentials);

        Assert.Null(resultUser);
    }
}