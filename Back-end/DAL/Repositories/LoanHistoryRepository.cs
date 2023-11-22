using PL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<LoanHistory> _dbSet;

        public LoanHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<LoanHistory>();
        }

        /// <summary>
        /// Retrieves a collection of loan history records for a specific product ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>
        /// A collection of LoanHistoryDTO representing loan history for the specified product.
        /// </returns>
        public async Task<(IEnumerable<object>, int)> GetLoanHistoryByProductId(int id, int page, int pageSize)
        {
            IQueryable<LoanHistory> query = _context.LoanHistory
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate);
            
            int numberOfLoanHistory = await query.CountAsync();
            int skipCount = Math.Max(0, (page - 1) * pageSize);

            IEnumerable<LoanHistoryResponse> LoanHistoryList = await query
                .Skip(skipCount)
                .Take(pageSize)
                .Select(loan => new LoanHistoryResponse
                {
                    Type = loan.Product.Type.ToString(),
                    SerialNumber = loan.Product.SerialNumber,
                    Name = loan.Employee.Name,
                    ExpirationDate = loan.Product.ExpirationDate,
                    PurchaseDate = loan.Product.PurchaseDate,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ProductId = loan.Product.ProductId,
                })
                .ToListAsync();
            
            return (LoanHistoryList, numberOfLoanHistory);    
        }

        /// <summary>
        /// Retrieves a collection of loan history records for a specific customer ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>
        /// A collection of LoanHistoryDTO representing loan history for the specified customer.
        /// </returns>
        public async Task<(IEnumerable<object>, int)> GetLoanHistoryByCustomerId(int id, int page, int pageSize)
        {
            IQueryable<LoanHistory> query = _context.LoanHistory
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .Where(l => l.Employee.EmployeeId == id)
                .OrderByDescending(l => l.LoanDate);
                
            int numberOfLoanHistory = await query.CountAsync();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
                
            IEnumerable<LoanHistoryResponse> LoanHistoryList = await query
                .Skip(skipCount)
                .Take(pageSize)
                .Select(loan => new LoanHistoryResponse
                {
                    Type = loan.Product.Type.ToString(),
                    SerialNumber = loan.Product.SerialNumber,
                    Name = loan.Employee.Name,
                    Email = loan.Employee.Email,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                })
                .ToListAsync();
            return (LoanHistoryList, numberOfLoanHistory);    
        }

        /// <summary>
        /// Retrieves the return date for a specific product ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>
        /// Nullable DateTime representing the return date for the specified product.
        /// </returns>
        public async Task<DateTime?> GetReturnDatesByProductId(int productId)
        {
            int loanHistoryId = await _dbSet
                .Where(l => l.Product.ProductId == productId)
                .Select(l => l.LoanHistoryId)
                .FirstOrDefaultAsync();

            if (loanHistoryId == 0)
            {
                return DateTime.Now;
            }

            DateTime? returnDate = await _dbSet
                .Where(l => l.Product.ProductId == productId)
                .OrderBy(l => l.LoanHistoryId)
                .Select(l => l.ReturnDate)
                .LastOrDefaultAsync();

            return returnDate;
        }

        /// <summary>
        /// Retrieves the latest loan history record for a specific product ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>
        /// The latest LoanHistory record for the specified product.
        /// </returns>
        public async Task<LoanHistory> GetLatestLoanHistoryByProductId(int id)
        {
            LoanHistory loanHistory = await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .OrderBy(l => l.LoanHistoryId)
                .Where(l => l.Product.ProductId == id)
                .LastOrDefaultAsync();

            if(loanHistory == null)
            {
                loanHistory.ReturnDate = null;
            }

            return loanHistory;
        }

        /// <summary>
        /// Updates an existing loan history record.
        /// </summary>
        /// <param name="lh">The LoanHistory object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateLoanHistory(LoanHistory lh)
        {
            LoanHistory loanHistory = new LoanHistory
            {
                Product = await _context.Products.FindAsync(lh.Product.ProductId),
                Employee = lh.Employee,
                ReturnDate = DateTime.Now,
                LoanDate = lh.LoanDate,
                LoanHistoryId = lh.LoanHistoryId
            };

            _dbSet.Update(loanHistory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a new loan history record.
        /// </summary>
        /// <param name="lh">The LoanHistory object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PostLoanHistory(LoanHistory lh)
        {
            LoanHistory loanHistory = new LoanHistory
            {
                Product = await _context.Products.FindAsync(lh.Product.ProductId),
                Employee = await _context.Employees.FindAsync(lh.Employee.EmployeeId),
                ReturnDate = lh.ReturnDate,
                LoanDate = lh.LoanDate
            };

            await _dbSet.AddAsync(loanHistory);
            await _context.SaveChangesAsync();
        }
    }
}
