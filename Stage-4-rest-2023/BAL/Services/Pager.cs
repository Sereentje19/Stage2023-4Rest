namespace Stage4rest2023.Services
{
    public class Pager
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Pager class with the specified parameters.
        /// </summary>
        /// <param name="totalItems">The total number of items to be paginated.</param>
        /// <param name="currentPage">The current page number (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 5).</param>
        public Pager(
            int totalItems,
            int currentPage = 1,
            int pageSize = 5)
        {
             Int32 totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

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
    }
}