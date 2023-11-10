using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;

namespace Stage4rest2023.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize);
        Task<Product> GetProductById(int id);
        Task PostProduct(Product product);
        Task PutProduct(Product product);
        Task DeleteProduct(int id);
    }
}