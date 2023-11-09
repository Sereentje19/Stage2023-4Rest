using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
{
    public interface ILoanHistoryRepository
    {
        IEnumerable<LoanHistoryDTO> GetLoanHistoryByProductId(int id);
        IEnumerable<LoanHistoryDTO> GetLoanHistoryByCustomerId(int id);
        DateTime? GetReturnDatesByProductId(int productId);
        LoanHistory GetLatestLoanHistoryByProductId(int id);
        void UpdateLoanHistory(LoanHistory loanHistory);
        void PostLoanHistory(LoanHistory loanHistory);
    }
}