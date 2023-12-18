using DAL.Models;

namespace BLL.Interfaces
{
    public interface ILoanHistoryService
    {
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByCustomerIdAsync(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductIdAsync(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id);
        Task CreateLoanHistoryAsync(LoanHistory loanHistory);
        Task UpdateLoanHistoryAsync(LoanHistory loanHistory);
    }
}
