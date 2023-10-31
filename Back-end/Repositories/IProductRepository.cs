using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll(string searchfield, ProductType? dropdown);
        Product GetById(int id);
        void Put(Product product);
        void Delete(int id);
    }
}