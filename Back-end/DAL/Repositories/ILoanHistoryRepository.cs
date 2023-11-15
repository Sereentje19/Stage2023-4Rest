using PL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
{
    public interface ILoanHistoryRepository
    {
        Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByProductId(int id);
        Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByCustomerId(int id);
        Task<DateTime?> GetReturnDatesByProductId(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductId(int id);
        Task UpdateLoanHistory(LoanHistory loanHistory);
        Task PostLoanHistory(LoanHistory loanHistory);
    }
}
