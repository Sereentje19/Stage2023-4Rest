using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Back_end.Services
{
    public interface IFilterService
    {
        // (IEnumerable<object>, Pager) FilterDocumentsAndCustomers(string searchfield, Models.Type? dropBoxType, int page = 1, int pageSize = 5);
    }
}