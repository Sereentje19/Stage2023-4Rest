using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;

        /// <summary>
        /// Initializes a new instance of the DocumentService class with the provided DocumentRepository.
        /// </summary>
        /// <param name="fr">The DocumentRepository used for document-related operations.</param>
        public FilterService(IFilterRepository fr)
        {
            _filterRepository = fr;
        }

        public object FilterDocumentsAndCustomers(string searchfield, Models.Type? dropBoxType)
        {
            return _filterRepository.FilterDocumentsAndCustomers(searchfield, dropBoxType);
        }
    }
}