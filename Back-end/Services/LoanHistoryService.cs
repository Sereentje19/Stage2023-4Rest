using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class LoanHistoryService : ILoanHistoryService
    {
        private readonly ILoanHistoryRepository _loanHistoryRepository;

        public LoanHistoryService(ILoanHistoryRepository loanHistoryRepository)
        {
            _loanHistoryRepository = loanHistoryRepository;
        }

        public IEnumerable<LoanHistory> GetLoanHistoryByProductId(int id)
        {
            return _loanHistoryRepository.GetLoanHistoryByProductId(id);
        }

        public IEnumerable<LoanHistory> GetLoanHistoryByCustomerId(int id)
        {
            return _loanHistoryRepository.GetLoanHistoryByCustomerId(id);
        }
        public DateTime? GetReturnDatesByProductId(int productId)
        {
            return _loanHistoryRepository.GetReturnDatesByProductId(productId);
        }
        public LoanHistory GetLatestLoanHistoryByProductId(int id)
        {
            return _loanHistoryRepository.GetLatestLoanHistoryByProductId(id);
        }

        public void UpdateLoanHistory(LoanHistory loanHistory)
        {
            _loanHistoryRepository.UpdateLoanHistory(loanHistory);
        }
        public void PostLoanHistory(LoanHistory loanHistory)
        {
            _loanHistoryRepository.PostLoanHistory(loanHistory);
        }
    }
}