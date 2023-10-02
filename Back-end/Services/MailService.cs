using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Back_end.Models;
using Microsoft.Extensions.Options;
using System.Drawing;
using MimeKit.Utils;


namespace Back_end.Services
{
    public class MailService : IMailService
    {
        // private string fromEmail = "serena.kenter@4-rest.nl"; // Your email address
        // private string toEmail = "serena.kenter@gmail.com"; // Recipient's email address
        // private string smtpHost = "smtp.office365.com"; // Gmail SMTP server
        // private int smtpPort = 587; // Port for Gmail SMTP
        // private string smtpUsername = "serena.kenter@4-rest.nl"; // Your Gmail username
        // private string smtpPassword = "Stage2023@4Rest"; // Your Gmail password

        private string toEmail = "serena.kenter@4-rest.nl"; // Recipient's email address
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        private MimePart SetImage(byte[] image)
        {
            var imagePart = new MimePart("image", "jpeg")
            {
                Content = new MimeContent(new MemoryStream(image), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "image.jpg",
                ContentId = MimeUtils.GenerateMessageId()
            };
            return imagePart;
        }

        private BodyBuilder SetBody(int weeks, string customerName, DateTime date, Models.Type type)
        {
            BodyBuilder emailBodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"<html>
                                    <head></head>
                                    <body>
                                    <p>Het volgende document zal over {weeks} weken komen te vervallen:</p>
                                    <p>Naam: {customerName}</p>
                                    <p>Verloop datum: {date:dd-MM-yyyy}</p>
                                    <p>Type document: {type}</p>
                                    <img src=""cid:imageId"" width=""100"">
                                    </body>
                                    </html>"
            };

            return emailBodyBuilder;
        }

        private void ConnectAndSendMail(MimeMessage emailMessage)
        {
            using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                mailClient.Send(emailMessage);
                mailClient.Disconnect(true);
            }
        }

        public void SendEmail(string customerName, DateTime date, Models.Type type, byte[] image, int weeks)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress("serena", toEmail);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = "Document vervalt Binnenkort!";

                    BodyBuilder emailBodyBuilder = SetBody(weeks, customerName, date, type);

                    MimePart imagePart = SetImage(image);

                    emailBodyBuilder.HtmlBody = emailBodyBuilder.HtmlBody.Replace("src=\"cid:imageId\"", $"src=\"cid:{imagePart.ContentId}\"");
                    emailBodyBuilder.LinkedResources.Add(imagePart);
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    ConnectAndSendMail(emailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
