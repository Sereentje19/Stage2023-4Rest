using DAL.Models;

namespace BLL.Interfaces
{
    public interface ILoanHistoryService
    {
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByProductId(int id, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByCustomerId(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductId(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductId(int id);
        Task UpdateLoanHistory(LoanHistory loanHistory);
        Task PostLoanHistory(LoanHistory loanHistory);
    }
}
