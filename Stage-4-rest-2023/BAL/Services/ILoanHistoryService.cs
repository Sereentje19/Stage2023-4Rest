using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface ILoanHistoryService
    {
        Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByProductId(int id);
        Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByCustomerId(int id);
        Task<DateTime?> GetReturnDatesByProductId(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductId(int id);
        Task UpdateLoanHistory(LoanHistory loanHistory);
        Task PostLoanHistory(LoanHistory loanHistory);
    }
}