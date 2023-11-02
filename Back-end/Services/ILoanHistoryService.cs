using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface ILoanHistoryService
    {
        IEnumerable<LoanHistory> GetLoanHistoryByProductId(int id);
        IEnumerable<LoanHistory> GetLoanHistoryByCustomerId(int id);
        DateTime? GetReturnDatesByProductId(int productId);
        LoanHistory GetLatestLoanHistoryByProductId(int id);
        void UpdateLoanHistory(LoanHistory loanHistory);
        void PostLoanHistory(LoanHistory loanHistory);
    }
}