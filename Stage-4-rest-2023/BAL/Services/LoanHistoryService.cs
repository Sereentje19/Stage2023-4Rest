using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Repositories;

namespace Stage4rest2023.Services
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
        public async Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByProductId(int id)
        {
            return await _loanHistoryRepository.GetLoanHistoryByProductId(id);
        }

        /// <summary>
        /// Retrieves loan history records based on the specified customer ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>A collection of LoanHistoryDTO representing loan history for the customer.</returns>
        public async Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByCustomerId(int id)
        {
            return await _loanHistoryRepository.GetLoanHistoryByCustomerId(id);
        }

        /// <summary>
        /// Retrieves return dates for a specific product ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>Nullable DateTime representing the return date.</returns>
        public async Task<DateTime?> GetReturnDatesByProductId(int productId)
        {
            return await _loanHistoryRepository.GetReturnDatesByProductId(productId);
        }

        /// <summary>
        /// Retrieves the latest loan history record based on the specified product ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The latest LoanHistory record for the product.</returns>
        public async Task<LoanHistory> GetLatestLoanHistoryByProductId(int id)
        {
            return await _loanHistoryRepository.GetLatestLoanHistoryByProductId(id);
        }

        /// <summary>
        /// Updates an existing loan history record.
        /// </summary>
        /// <param name="loanHistory">The LoanHistory object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateLoanHistory(LoanHistory loanHistory)
        {
            await _loanHistoryRepository.UpdateLoanHistory(loanHistory);
        }

        /// <summary>
        /// Posts a new loan history record.
        /// </summary>
        /// <param name="loanHistory">The LoanHistory object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PostLoanHistory(LoanHistory loanHistory)
        {
            await _loanHistoryRepository.PostLoanHistory(loanHistory);
        }
    }
}