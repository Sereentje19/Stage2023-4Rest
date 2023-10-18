namespace Back_end.Services
{
    public interface IMailService
    {
        void SendEmail(string customerName, string fileType, DateTime date, Models.DocumentType type, byte[] image, int weeks);
    }
}