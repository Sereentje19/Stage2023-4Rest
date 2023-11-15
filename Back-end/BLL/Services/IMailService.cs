using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IMailService
    {
        void SendEmail(string customerName, string fileType, DateTime date, PL.Models.DocumentType type, byte[] image, int weeks);
    }
}
