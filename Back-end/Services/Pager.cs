using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_end.Services
{
    public class Pager
    {
        public Pager(
            int totalItems,
            int currentPage = 1,
            int pageSize = 5)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
    }
    
}