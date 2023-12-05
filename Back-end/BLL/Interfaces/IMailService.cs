using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IMailService
    {
        void SendDocumentExpirationEmail(string body, string fileType, byte[] image, string subject);
        void SendPasswordEmail(string body, string email, string subject, string customerName);
    }
}
