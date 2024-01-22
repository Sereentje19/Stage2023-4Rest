using BLL.Interfaces;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class PasswordResetControllerTests
{
        [Fact]
        public async Task CreateResetCodeAsync_ReturnsOkResult()
        {
            const string email = "test@example.com";
            Mock<IPasswordResetService> passwordResetServiceMock = new Mock<IPasswordResetService>();
            PasswordResetController controller = new PasswordResetController(passwordResetServiceMock.Object);

            IActionResult result = await controller.CreateResetCodeAsync(email);
            Assert.IsType<OkObjectResult>(result);

            passwordResetServiceMock.Verify(s => s.CreateResetCodeAsync(email), Times.Once);
        }
        
        [Fact]
        public async Task CheckEnteredCodeAsync_ReturnsOkResult()
        {
            const string email = "test@example.com";
            const string code = "123456";
            Mock<IPasswordResetService> passwordResetServiceMock = new Mock<IPasswordResetService>();
            PasswordResetController controller = new PasswordResetController(passwordResetServiceMock.Object);

            IActionResult result = await controller.CheckEnteredCodeAsync(email, code);
            Assert.IsType<OkObjectResult>(result);

            passwordResetServiceMock.Verify(s => s.CheckEnteredCodeAsync(email, code), Times.Once);
        }

        [Fact]
        public async Task CreatePasswordAsync_ReturnsOkResult()
        {
            CreatePasswordRequestDto requestDto = new CreatePasswordRequestDto
            {
                Email = "test@example.com",
                Password1 = "newPassword1",
                Password2 = "newPassword2",
                Code = "123456"
            };

            Mock<IPasswordResetService> passwordResetServiceMock = new Mock<IPasswordResetService>();
            PasswordResetController controller = new PasswordResetController(passwordResetServiceMock.Object);

            IActionResult result = await controller.CreatePasswordAsync(requestDto);
            Assert.IsType<OkObjectResult>(result);

            passwordResetServiceMock.Verify(s => s.CreatePasswordAsync(requestDto), Times.Once);
        }
        
        [Fact]
        public async Task UpdatePasswordAsync_ReturnsOkResult()
        {
            UpdatePasswordRequestDto updatePasswordRequestDto = new UpdatePasswordRequestDto
            {
                Email = "test@example.com",
                UserId = 1,
            };

            Mock<IPasswordResetService> passwordResetServiceMock = new Mock<IPasswordResetService>();
            PasswordResetController controller = new PasswordResetController(passwordResetServiceMock.Object);

            IActionResult result = await controller.UpdatePasswordAsync(updatePasswordRequestDto);
            Assert.IsType<OkObjectResult>(result);

            passwordResetServiceMock.Verify(s => s.UpdatePasswordAsync(updatePasswordRequestDto), Times.Once);
        }
        
}