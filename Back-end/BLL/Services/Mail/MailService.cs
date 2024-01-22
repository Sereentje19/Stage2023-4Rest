using BLL.Interfaces;
using DAL.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BLL.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        /// <summary>
        /// Creates a MIME part for embedding an image in the email.
        /// </summary>
        /// <param name="image">The byte array representing the image data.</param>
        /// <param name="fileType"></param>
        /// <returns>The MIME part representing the embedded image.</returns>
        private static MimePart SetImage(byte[] image, string fileType)
        {
            MimePart imageAttachment = new MimePart(fileType)
            {
                Content = new MimeContent(new MemoryStream(image), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "image.jpg"
            };
            return imageAttachment;
        }

        /// <summary>
        /// Sets the body of an email message with optional text content and image attachment.
        /// </summary>
        /// <param name="emailMessage">The MimeMessage object representing the email message.</param>
        /// <param name="body">The text content to be included in the email body.</param>
        /// <param name="image">The byte array representing the image attachment (optional, set to null if not applicable).</param>
        /// <param name="fileType">The file type of the image attachment (e.g., "jpeg", "png").</param>
        private static void SetBody(MimeMessage emailMessage, string body, byte[] image, string fileType)
        {
            TextPart textPart = new TextPart("plain")
            {
                Text = body
            };

            Multipart multipart = new Multipart("mixed");
            multipart.Add(textPart);

            // Add the image as an attachment
            if (image != null)
            {
                MimePart imageAttachment = SetImage(image, fileType);
                multipart.Add(imageAttachment);
            }

            emailMessage.Body = multipart;
        }


        /// <summary>
        /// Connects to the SMTP server, authenticates, and sends the email message.
        /// </summary>
        /// <param name="emailMessage">The MimeMessage to be sent.</param>
        private async void ConnectAndSendMail(MimeMessage emailMessage)
        {
            using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, 
                    MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await mailClient.SendAsync(emailMessage);
                await mailClient.DisconnectAsync(true);
            }
        }

        /// <summary>
        /// Sends an email with document expiration information to a specified recipient.
        /// </summary>
        /// <param name="customerName">The name of the customer.</param>
        /// <param name="email"></param>
        private MimeMessage SetMailSettings(string customerName, string email)
        {
            MimeMessage emailMessage = new MimeMessage();
            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(customerName, email);
            emailMessage.To.Add(emailTo);
            return emailMessage;
        }

        /// <summary>
        /// Sends an email notification for an expiring document.
        /// </summary>
        /// <param name="body">The content of the email body.</param>
        /// <param name="fileType">The file type of an optional image attachment (e.g., "jpeg", "png").</param>
        /// <param name="image">The byte array representing the optional image attachment.</param>
        /// <param name="subject">The subject of the email.</param>
        public void SendDocumentExpirationEmail(string body, string fileType, byte[] image, string subject)
        {
            MimeMessage emailMessage = SetMailSettings("Administratie", _mailSettings.ReceiverEmail);
            emailMessage.Subject = "Document vervalt Binnenkort!";

            SetBody(emailMessage, body, image, fileType);
            ConnectAndSendMail(emailMessage);
        }

        /// <summary>
        /// Sends an email with a password-related notification to a customer.
        /// </summary>
        /// <param name="body">The content of the email body.</param>
        /// <param name="email">The email address of the customer.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="customerName">The name of the customer receiving the email.</param>
        public void SendPasswordEmail(string body, string email, string subject, string customerName)
        {
            MimeMessage emailMessage = SetMailSettings("customerName", email);
            emailMessage.Subject = subject;

            SetBody(emailMessage, body, null, null);
            ConnectAndSendMail(emailMessage);
        }

    }
}