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

        public IEnumerable<LoanHistory> GetAll()
        {
            return _loanHistoryRepository.GetAll();
        }
        public IEnumerable<LoanHistory> GetByProductId(int id){
            return _loanHistoryRepository.GetByProductId(id);
        }
        public LoanHistory GetFirstByProductId(int id){
            return _loanHistoryRepository.GetFirstByProductId(id);
        }

        public void ReturnProduct(LoanHistory loanHistory)
        {
             _loanHistoryRepository.ReturnProduct(loanHistory);
        }
        public void PostLoanHistory(LoanHistory loanHistory)
        {
            _loanHistoryRepository.PostLoanHistory(loanHistory);
        }
    }
}