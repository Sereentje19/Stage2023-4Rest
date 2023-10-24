using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface IProductService
    {
        (IEnumerable<object>, Pager) GetAll(int page, int pageSize);
        Product GetById(int id);
    }
}