using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Customer> _dbSet;

        /// <summary>
        /// Initializes a new instance of the CustomerRepository class with the provided NotificationContext.
        /// </summary>
        /// <param name="context">The NotificationContext used for database interactions.</param>
        public FilterRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Customer>();
        }


        public object FilterDocumentsAndCustomers(string searchfield, Models.Type? dropBoxType)
        {
            var query = from document in _context.Documents
                        join customer in _context.Customers on document.CustomerId equals customer.CustomerId
                        where string.IsNullOrEmpty(searchfield) ||
                            customer.Name.Contains(searchfield) ||
                            customer.Email.Contains(searchfield) ||
                            customer.CustomerId.ToString().Contains(searchfield)
                        where dropBoxType == Models.Type.Not_Selected || document.Type.Equals(dropBoxType)
                        orderby document.Date
                        select new
                        {
                            Document = document,
                            Customer = customer
                        };

            var result = new
            {
                Documents = query.Select(item => item.Document).ToList(),
                Customers = query.Select(item => item.Customer).Distinct().ToList()
            };

            return result;
        }



    }
}