using Azure;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize);
        List<string> GetProductTypeStrings();
        Task<Product> GetProductById(int id);
        Task PostProduct(Product product);
        Task PutProduct(Product product);
        Task DeleteProduct(int id);
    }
}
