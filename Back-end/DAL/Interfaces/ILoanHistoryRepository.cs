using DAL.Models;

namespace DAL.Interfaces
{
    public interface ILoanHistoryRepository
    {
        Task<(IEnumerable<object>, int)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize);
        Task<(IEnumerable<object>, int)> GetLoanHistoryByCustomerIdAsync(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductIdAsync(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id);
        Task CreateLoanHistoryAsync(LoanHistory loanHistory);
        Task UpdateLoanHistoryAsync(LoanHistory loanHistory);
    }
}
