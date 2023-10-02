using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Back_end.Models;
using Microsoft.Extensions.Options;


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

        public void SendEmail(string customerName, DateTime date, Models.Type type)
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

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = $"Het volgende document zal over 6 weken komen te vervallen:\n"
                     + customerName + "\n" + date.ToString() + "\n" + type.ToString();

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
