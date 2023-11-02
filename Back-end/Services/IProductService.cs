using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface IProductService
    {
        (IEnumerable<object>, Pager) GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize);
        Product GetProductById(int id);
        void PostProduct(Product product);
        void PutProduct(Product product);
        void DeleteProduct(int id);
    }
}