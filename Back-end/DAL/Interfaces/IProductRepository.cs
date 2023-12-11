using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<object>, int)> GetAllProducts(string searchfield, string dropdown, int page, int pageSize);
        Task<IEnumerable<ProductType>> GetProductTypes();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task PutProduct(Product product);
        Task DeleteProduct(int id);
    }
}
