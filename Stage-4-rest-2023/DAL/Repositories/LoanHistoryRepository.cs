using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Microsoft.EntityFrameworkCore;

namespace Stage4rest2023.Repositories
{
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<LoanHistory> _dbSet;

        public LoanHistoryRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<LoanHistory>();
        }


        public IEnumerable<LoanHistory> GetLoanHistoryByProductId(int id)
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
                .ToList();
        }

        public IEnumerable<LoanHistory> GetLoanHistoryByCustomerId(int id)
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .Where(l => l.Customer.CustomerId == id)
                .OrderByDescending(l => l.LoanDate)
                .ToList();
        }

        public DateTime? GetReturnDatesByProductId(int productId)
        {
            return _dbSet
                .Where(l => l.Product.ProductId == productId)
                .OrderByDescending(l => l.LoanDate)
                .Select(l => l.ReturnDate)
                .FirstOrDefault();
        }


        public LoanHistory GetLatestLoanHistoryByProductId(int id)
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
                .FirstOrDefault();
        }

        public void UpdateLoanHistory(LoanHistory lh)
        {
            try
            {
                if (lh == null)
                {
                    throw new Exception("geen item gevonden.");
                }

                LoanHistory loanHistory = new LoanHistory
                {
                    Product = _context.Products.Find(lh.Product.ProductId),
                    Customer = lh.Customer,
                    ReturnDate = DateTime.Now,
                    LoanDate = lh.LoanDate,
                    LoanHistoryId = lh.LoanHistoryId
                };

                _dbSet.Update(loanHistory);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void PostLoanHistory(LoanHistory lh)
        {
            try
            {
                LoanHistory loanHistory = new LoanHistory
                {
                    Product = _context.Products.Find(lh.Product.ProductId),
                    Customer = _context.Customers.Find(lh.Customer.CustomerId),
                    ReturnDate = lh.ReturnDate,
                    LoanDate = lh.LoanDate
                };

                _dbSet.Add(loanHistory);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }
    }
}