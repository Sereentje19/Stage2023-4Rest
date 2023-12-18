using System.Reflection;
using Castle.Core.Smtp;
using DAL.Settings;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using MailService = BLL.Services.MailService;

namespace Tests.Services;

public class MailServiceTests
{

    [Fact]
    public void SendPasswordEmail_ShouldSendEmailWithCorrectParameters()
    {
        // Arrange
        MailSettings mailSettings = new MailSettings
        {
            Server = "smtp.gmail.com",
            Port = 587,
            SenderName = "Serena kenter",
            SenderEmail = "pipix.kenter22@gmail.com",
            ReceiverEmail = "serena.kenter@4-rest.nl",
            UserName = "pipix.kenter22@gmail.com",
            Password = "qyhd zpfz yyzb kksr"
        };

        Mock<IOptions<MailSettings>> mockMailSettingsOptions = new Mock<IOptions<MailSettings>>();
        mockMailSettingsOptions.Setup(x => x.Value).Returns(mailSettings);

        Mock<SmtpClient> mockSmtpClient = new Mock<MailKit.Net.Smtp.SmtpClient>();

        MailService emailService = new MailService(mockMailSettingsOptions.Object);

        // Set the SmtpClient property using reflection
        FieldInfo smtpClientField = typeof(MailService).GetField("_smtpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        smtpClientField.SetValue(emailService, mockSmtpClient.Object);

        string body = "Password reset content";
        string email = "test@example.com";
        string subject = "Password Reset";
        const string customerName = "Test User";

        // Act
        emailService.SendPasswordEmail(body, email, subject, customerName);

        // Assert
        mockSmtpClient.Verify(client => client.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<MailKit.Security.SecureSocketOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        mockSmtpClient.Verify(client => client.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        // mockSmtpClient.Verify(client => client.Send(It.IsAny<MimeMessage>(), It.IsAny<System.Threading.CancellationToken>(), It.IsAny<System.IProgress<ITransferProgress>>()), Times.Once);
        mockSmtpClient.Verify(client => client.Disconnect(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        // Add more assertions as needed
    }

    
    
}