using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;

namespace Stage4rest2023.Repositories
{
    public interface ILoanHistoryRepository
    {
        IEnumerable<LoanHistory> GetLoanHistoryByProductId(int id);
        IEnumerable<LoanHistory> GetLoanHistoryByCustomerId(int id);
        DateTime? GetReturnDatesByProductId(int productId);
        LoanHistory GetLatestLoanHistoryByProductId(int id);
        void UpdateLoanHistory(LoanHistory loanHistory);
        void PostLoanHistory(LoanHistory loanHistory);
    }
}