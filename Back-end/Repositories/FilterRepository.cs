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
            var filteredCustomerIds = _context.Customers.AsQueryable();
            var filteredDocuments = _context.Documents.AsQueryable();

            if (dropBoxType != Models.Type.Not_Selected)
            {
                filteredDocuments = filteredDocuments
                    .Where(document =>
                        document.Type.Equals(dropBoxType)
                    );

                    filteredCustomerIds = filteredCustomerIds
                    .Where(customer => filteredDocuments.Any(d => d.CustomerId == customer.CustomerId));
                

                Console.WriteLine(dropBoxType);
            }

            if (!string.IsNullOrEmpty(searchfield))
            {
                filteredCustomerIds = filteredCustomerIds
                    .Where(customer =>
                        customer.Name.Contains(searchfield) ||
                        customer.Email.Contains(searchfield) ||
                        customer.CustomerId.ToString().Contains(searchfield)
                    );
                    
                filteredDocuments = filteredDocuments
                    .Where(document => filteredCustomerIds.Any(c => c.CustomerId == document.CustomerId));
                
                Console.WriteLine("blabla");
            }

            var result = new
            {
                Documents = filteredDocuments.ToList(),
                Customers = _context.Customers
            .Where(customer => filteredCustomerIds.Any(c => c.CustomerId == customer.CustomerId))
            .ToList()
            };

            return result;
        }


    }
}