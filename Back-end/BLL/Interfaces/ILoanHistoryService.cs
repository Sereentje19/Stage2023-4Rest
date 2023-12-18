using DAL.Models;
using DAL.Models.Requests;

namespace BLL.Interfaces
{
    public interface ILoanHistoryService
    {
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByCustomerIdAsync(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductIdAsync(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id);
        Task CreateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
        Task UpdateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
    }
}
