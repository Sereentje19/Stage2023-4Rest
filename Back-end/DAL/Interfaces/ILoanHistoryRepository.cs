﻿using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace DAL.Interfaces
{
    public interface ILoanHistoryRepository
    {
        Task<(IEnumerable<object>, int)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize);
        Task<(IEnumerable<object>, int)> GetLoanHistoryByEmployeeIdAsync(int id, int page, int pageSize);
        Task<DateTime?> GetReturnDatesByProductIdAsync(int productId);
        Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id);
        Task CreateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
        Task UpdateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest);
    }
}
