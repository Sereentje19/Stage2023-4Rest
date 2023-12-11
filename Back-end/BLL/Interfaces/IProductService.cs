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
        Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, string dropdown, int page, int pageSize);

        Task<(IEnumerable<object>, Pager)> GetAllDeletedProducts(string searchfield, string dropdown,
            int page, int pageSize);
        Task<IEnumerable<ProductType>> GetProductTypes();
        Task<Product> GetProductById(int id);
        Task PostProduct(Product product);
        Task PutProduct(Product product);
        Task PutIsDeleted(Product product);
        Task DeleteProduct(int id);
    }
}
