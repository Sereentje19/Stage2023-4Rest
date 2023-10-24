using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface ILoanHistoryService
    {
        IEnumerable<LoanHistory> GetAll();
        IEnumerable<LoanHistory> GetByProductId(int id);
        LoanHistory GetFirstByProductId(int id);
        void ReturnProduct(LoanHistory loanHistory);
        void PostLoanHistory(LoanHistory loanHistory);
    }
}