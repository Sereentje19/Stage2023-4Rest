using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace BLL.Interfaces
{
    public interface ILoanHistoryService
    {
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetLoanHistoryByEmployeeIdAsync(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductIdAsync(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id);
        Task CreateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
        Task UpdateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
    }
}
