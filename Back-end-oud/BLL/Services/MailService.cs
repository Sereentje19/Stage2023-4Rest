using Microsoft.Extensions.Options;
using MimeKit;
using PL.Models;

namespace BLL.Services
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
        /// <returns>The MIME part representing the embedded image.</returns>
        private MimePart SetImage(byte[] image, string fileType)
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
        /// Sets the HTML body of the email message with specific content and embeds an image.
        /// </summary>
        /// <param name="weeks">The number of weeks until document expiration.</param>
        /// <param name="customerName">The name of the customer.</param>
        /// <param name="date">The expiration date of the document.</param>
        /// <param name="type">The type of the document.</param>
        /// <param name="emailMessage">The MimeMessage to which the body will be set.</param>
        /// <param name="image">The byte array representing the embedded image.</param>
        private void SetBody(int weeks, string customerName, DateTime date, DocumentType type, MimeMessage emailMessage, byte[] image, string fileType)
        {
            TextPart textPart = new TextPart("plain")
            {
                Text = $"Het volgende document zal over {weeks} weken komen te vervallen:" +
                       $"\nNaam: {customerName}" +
                       $"\nVerloop datum: {date:dd-MM-yyyy}" +
                       $"\nType document: {type.ToString().Replace("_", " ")}\n\n"
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
        public void SendEmail(string customerName, string fileType, DateTime date, DocumentType type, byte[] image, int weeks)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress("Administratie", _mailSettings.ReceiverEmail);
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
