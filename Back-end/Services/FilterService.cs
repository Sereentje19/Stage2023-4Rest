using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Models.DTOs;
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

        // public (IEnumerable<object>, Pager) FilterDocumentsAndCustomers(string searchfield, Models.Type? dropBoxType, int page = 1, int pageSize = 5)
        // {
        //     var documents = _filterRepository.FilterDocumentsAndCustomers(searchfield, dropBoxType);

        //     int totalArchivedDocuments = documents.Count();
        //     int skipCount = Math.Max(0, (page - 1) * pageSize);
        //     var pager = new Pager(totalArchivedDocuments, page, pageSize);

        //     var pagedDocuments = documents
        //         .Skip(skipCount)
        //         .Take(pageSize)
        //         .Select(doc => new
        //         {
        //             doc.DocumentId,
        //             doc.Date,
        //             doc.CustomerId,
        //             Type = doc.Type.ToString().Replace("_", " ")
        //         })
        //         .ToList();

        //      return (pagedDocuments.Cast<object>(), pager);
        // }
    }
}