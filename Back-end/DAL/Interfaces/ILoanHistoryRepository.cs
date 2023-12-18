using DAL.Models;

namespace DAL.Interfaces
{
    public interface ILoanHistoryRepository
    {
        Task<(IEnumerable<object>, int)> GetLoanHistoryByProductId(int id, int page, int pageSize);
        Task<(IEnumerable<object>, int)> GetLoanHistoryByCustomerId(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductId(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductId(int id);
        Task UpdateLoanHistory(LoanHistory loanHistory);
        Task PostLoanHistory(LoanHistory loanHistory);
    }
}
