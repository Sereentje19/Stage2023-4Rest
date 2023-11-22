using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IMailService
    {
        void SendDocumentExpirationEmail(string customerName, string fileType, DateTime date, PL.Models.DocumentType type, byte[] image, int weeks);
        void SendPasswordEmail(string body, string email, string subject);
    }
}
