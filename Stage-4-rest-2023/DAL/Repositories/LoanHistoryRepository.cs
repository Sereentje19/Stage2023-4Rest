using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Microsoft.EntityFrameworkCore;
using Stage4rest2023.Models.DTOs;
using DbContext = Stage4rest2023.Models.DbContext;

namespace Stage4rest2023.Repositories
{
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<LoanHistory> _dbSet;

        public LoanHistoryRepository(DbContext context)
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
        public async Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByProductId(int id)
        {
            return await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
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
        }

        /// <summary>
        /// Retrieves a collection of loan history records for a specific customer ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>
        /// A collection of LoanHistoryDTO representing loan history for the specified customer.
        /// </returns>
        public async Task<IEnumerable<LoanHistoryResponse>> GetLoanHistoryByCustomerId(int id)
        {
            return await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .Where(l => l.Employee.EmployeeId == id)
                .OrderByDescending(l => l.LoanDate)
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
                .OrderByDescending(l => l.LoanDate)
                .Select(l => l.ReturnDate)
                .FirstOrDefaultAsync();

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
            return await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Product)
                .Where(l => l.Product.ProductId == id)
                .OrderByDescending(l => l.LoanDate)
                .FirstOrDefaultAsync();
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