namespace BLL.Interfaces
{
    public interface IMailService
    {
        void SendDocumentExpirationEmail(string body, string fileType, byte[] image, string subject);
        void SendPasswordEmail(string body, string email, string subject, string customerName);
    }
}
