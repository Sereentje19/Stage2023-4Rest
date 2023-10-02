using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Services
{
    public interface IMailService
    {
        void SendEmail(string customerName, DateTime date, Models.Type type, byte[] image, int weeks);
    }
}