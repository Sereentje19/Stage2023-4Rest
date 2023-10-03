using MimeKit;
using Back_end.Models;
using Microsoft.Extensions.Options;
using MimeKit.Utils;


namespace Back_end.Services
{
    public class MailService : IMailService
    {
        private string toEmail = "serena.kenter@4-rest.nl";
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

        private void SetBody(int weeks, string customerName, DateTime date, Models.Type type, MimeMessage emailMessage, byte[] image)
        {
            BodyBuilder emailBodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"<html>
                            <style>
                            .email-image {{ width: 200px;  }} 
                            </style>
                            <body>
                            <p>Het volgende document zal over {weeks} weken komen te vervallen:</p>
                            <p>Naam: {customerName}</p>
                            <p>Verloop datum: {date:dd-MM-yyyy}</p>
                            <p>Type document: {type}</p>
                            <img src=""cid:imageId"" class=""email-image"">
                            </body>
                            </html>"
            };

            MimePart imagePart = SetImage(image);

            emailBodyBuilder.HtmlBody = emailBodyBuilder.HtmlBody.Replace("src=\"cid:imageId\"", $"src=\"cid:{imagePart.ContentId}\"");
            emailBodyBuilder.LinkedResources.Add(imagePart);
            emailMessage.Body = emailBodyBuilder.ToMessageBody();
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

                    SetBody(weeks, customerName, date, type, emailMessage, image);

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