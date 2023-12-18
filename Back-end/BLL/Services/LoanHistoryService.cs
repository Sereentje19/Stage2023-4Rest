using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class LoanHistoryService : ILoanHistoryService
    {
        private readonly ILoanHistoryRepository _loanHistoryRepository;

        public LoanHistoryService(ILoanHistoryRepository loanHistoryRepository)
        {
            _loanHistoryRepository = loanHistoryRepository;
        }

        /// <summary>
        /// Retrieves loan history records based on the specified product ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>A collection of LoanHistoryDTO representing loan history for the product.</returns>
        public async Task<(IEnumerable<object>, Pager)> GetLoanHistoryByProductIdAsync(int id, int page, int pageSize)
        {
            (IEnumerable<object> pagedHistory, int numberOfHistory) = await _loanHistoryRepository.GetLoanHistoryByProductIdAsync(id, page, pageSize);
            Pager pager = new Pager(numberOfHistory, page, pageSize);
            return (pagedHistory, pager);
        }

        /// <summary>
        /// Retrieves loan history records based on the specified customer ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>A collection of LoanHistoryDTO representing loan history for the customer.</returns>
        public async Task<(IEnumerable<object>, Pager)> GetLoanHistoryByCustomerIdAsync(int id, int page, int pageSize)
        {
            (IEnumerable<object> pagedHistory, int numberOfHistory) = await _loanHistoryRepository.GetLoanHistoryByCustomerIdAsync(id, page, pageSize);
            Pager pager = new Pager(numberOfHistory, page, pageSize);
            return (pagedHistory, pager);
        }

        /// <summary>
        /// Retrieves return dates for a specific product ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>Nullable DateTime representing the return date.</returns>
        public async Task<DateTime?> GetReturnDatesByProductIdAsync(int productId)
        {
            return await _loanHistoryRepository.GetReturnDatesByProductIdAsync(productId);
        }

        /// <summary>
        /// Retrieves the latest loan history record based on the specified product ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The latest LoanHistory record for the product.</returns>
        public async Task<LoanHistory> GetLatestLoanHistoryByProductIdAsync(int id)
        {
            return await _loanHistoryRepository.GetLatestLoanHistoryByProductIdAsync(id);
        }

        /// <summary>
        /// Updates an existing loan history record.
        /// </summary>
        /// <param name="loanHistory">The LoanHistory object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateLoanHistoryAsync(LoanHistory loanHistory)
        {
            await _loanHistoryRepository.UpdateLoanHistoryAsync(loanHistory);
        }

        /// <summary>
        /// Posts a new loan history record.
        /// </summary>
        /// <param name="loanHistory">The LoanHistory object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task CreateLoanHistoryAsync(LoanHistory loanHistory)
        {
            await _loanHistoryRepository.CreateLoanHistoryAsync(loanHistory);
        }
    }
}
