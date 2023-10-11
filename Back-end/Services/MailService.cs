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

        /// <summary>
        /// Initializes a new instance of the MailService class.
        /// </summary>
        /// <param name="mailSettingsOptions">The configuration options for email settings.</param>
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        /// <summary>
        /// Creates a MIME part for embedding an image in the email.
        /// </summary>
        /// <param name="image">The byte array representing the image data.</param>
        /// <returns>The MIME part representing the embedded image.</returns>
        private MimePart SetImage(byte[] image, string fileType)
        {
            var imagePart = new MimePart()
            {
                Content = new MimeContent(new MemoryStream(image), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = fileType,
                ContentId = MimeUtils.GenerateMessageId()
            };
            return imagePart;
        }

        /// <summary>
        /// Sets the HTML body of the email message with specific content and embeds an image.
        /// </summary>
        /// <param name="weeks">The number of weeks until document expiration.</param>
        /// <param name="customerName">The name of the customer.</param>
        /// <param name="date">The expiration date of the document.</param>
        /// <param name="type">The type of the document.</param>
        /// <param name="emailMessage">The MimeMessage to which the body will be set.</param>
        /// <param name="image">The byte array representing the embedded image.</param>
        private void SetBody(int weeks, string customerName, DateTime date, Models.Type type, MimeMessage emailMessage, byte[] image, string fileType)
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
                            <p>Type document: {type.ToString().Replace("_", " ")}</p>
                            <img src=""cid:imageId"" class=""email-image"">
                            </body>
                            </html>"
            };

            MimePart imagePart = SetImage(image, fileType);

            emailBodyBuilder.HtmlBody = emailBodyBuilder.HtmlBody.Replace("src=\"cid:imageId\"", $"src=\"cid:{imagePart.ContentId}\"");
            emailBodyBuilder.LinkedResources.Add(imagePart);
            emailMessage.Body = emailBodyBuilder.ToMessageBody();
        }

        /// <summary>
        /// Connects to the SMTP server, authenticates, and sends the email message.
        /// </summary>
        /// <param name="emailMessage">The MimeMessage to be sent.</param>
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

        /// <summary>
        /// Sends an email with document expiration information to a specified recipient.
        /// </summary>
        /// <param name="customerName">The name of the customer.</param>
        /// <param name="date">The expiration date of the document.</param>
        /// <param name="type">The type of the document.</param>
        /// <param name="image">The byte array representing the embedded image.</param>
        /// <param name="weeks">The number of weeks until document expiration.</param>
        public void SendEmail(string customerName, string fileType, DateTime date, Models.Type type, byte[] image, int weeks)
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

                    SetBody(weeks, customerName, date, type, emailMessage, image, fileType);
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
