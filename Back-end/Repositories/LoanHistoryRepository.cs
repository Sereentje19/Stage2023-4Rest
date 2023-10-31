using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
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

        public IEnumerable<LoanHistory> GetAll()
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .ToList();
        }

        public IEnumerable<LoanHistory> GetByProductId(int id)
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
                .ToList();
        }

        public IEnumerable<LoanHistory> GetByCustomerId(int id)
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
                .First();
        }


        public LoanHistory GetFirstByProductId(int id)
        {
            return _dbSet
                .Include(l => l.Customer)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
                .First();
        }

        public void ReturnProduct(LoanHistory lh)
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

        public void PostLoanHistory(LoanHistory lh)
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
    }
}